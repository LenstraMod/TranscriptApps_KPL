using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;
using NAudio.Wave;

namespace Frontend
{
    public partial class Transkrip_UI : Form
    {
        // Variabel NAudio — null saat tidak sedang merekam
        private WaveInEvent? waveIn;
        private WaveFileWriter? waveWriter;

        // Path file audio hasil rekaman — null sebelum START diklik
        private string? outputAudioPath;

        // ID jadwal yang sedang diproses, diterima dari card yang diklik di Home
        private readonly string _scheduleId;

        // Role user yang login, menentukan tampilan transcript mana yang lebih relevan
        private readonly string _userRole;

        // Constructor utama: dipanggil oleh UserControl1 saat card diklik
        public Transkrip_UI(string scheduleId, string userRole)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            _userRole = userRole;

            txtDraft.Clear();
            txtFinal.Clear();
        }

        // Fallback constructor tanpa parameter (untuk kompatibilitas)
        public Transkrip_UI() : this("", SessionManager.UserRole) { }

        // ---- TOMBOL START REKAM ----
        private void button1_Click(object sender, EventArgs e)
        {
            // Cegah klik START dua kali saat masih merekam
            if (waveIn != null)
            {
                MessageBox.Show("Perekaman sudah berjalan.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string folderAudio = Path.Combine(Application.StartupPath, "audio");
                if (!Directory.Exists(folderAudio))
                    Directory.CreateDirectory(folderAudio);

                // Timestamp di nama file mencegah file lama tertimpa
                string namaFile = $"rekaman_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
                outputAudioPath = Path.Combine(folderAudio, namaFile);

                waveIn = new WaveInEvent();
                waveIn.WaveFormat = new WaveFormat(44100, 1); // mono, 44.1kHz
                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.RecordingStopped += WaveIn_RecordingStopped;

                waveWriter = new WaveFileWriter(outputAudioPath, waveIn.WaveFormat);
                waveIn.StartRecording();

                // Tunjukkan status rekaman di UI — TANPA MessageBox karena itu memblokir UI thread
                // dan mencegah user mengklik STOP sampai dialog ditutup
                button1.Text = "● Merekam...";
                button1.BackColor = Color.LightCoral;
                button1.Enabled = false;
                buttonStop.Enabled = true;
            }
            catch (Exception ex)
            {
                // Bersihkan state jika inisialisasi gagal di tengah jalan
                waveWriter?.Dispose(); waveWriter = null;
                waveIn?.Dispose(); waveIn = null;
                MessageBox.Show("Gagal memulai perekaman: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Dipanggil NAudio tiap ada chunk suara baru — tulis langsung ke file WAV
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveWriter?.Write(e.Buffer, 0, e.BytesRecorded);
            waveWriter?.Flush();
        }

        // ---- TOMBOL STOP REKAM ----
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (waveIn == null)
            {
                MessageBox.Show("Tidak ada perekaman yang sedang berjalan.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            waveIn.StopRecording();
        }

        // Dipanggil otomatis NAudio setelah StopRecording() selesai
        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            // Lepaskan resource audio
            waveWriter?.Dispose(); waveWriter = null;
            waveIn?.Dispose(); waveIn = null;

            // Capture path SEKARANG sebelum BeginInvoke dijalankan.
            // Tanpa ini, jika user langsung klik START lagi, outputAudioPath
            // akan tertimpa path rekaman baru sebelum UploadAndTranscribe sempat jalan.
            string? capturedPath = outputAudioPath;
            outputAudioPath = null; // reset agar state bersih untuk rekaman berikutnya

            if (string.IsNullOrEmpty(capturedPath))
            {
                // Tetap pakai BeginInvoke agar MessageBox muncul di UI thread
                this.BeginInvoke(new Action(() =>
                    MessageBox.Show("File audio tidak ditemukan.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)));
                return;
            }

            // Semua UI logic (reset button + upload) HARUS jalan di UI thread.
            // WaveIn_RecordingStopped dipanggil dari thread NAudio — tanpa BeginInvoke,
            // MessageBox bisa muncul di belakang window dan tidak terlihat user.
            this.BeginInvoke(new Action(async () =>
            {
                // Reset tombol ke kondisi awal
                button1.Text = "Start";
                button1.BackColor = SystemColors.ButtonFace;
                button1.Enabled = true;
                buttonStop.Enabled = true;

                await UploadAndTranscribe(capturedPath);
            }));
        }

        // Mengunggah file WAV ke backend untuk diproses Gemini AI
        // audioPath: path hasil capture saat rekaman berhenti — sudah aman dari race condition
        private async Task UploadAndTranscribe(string audioPath)
        {
            // Guard ganda: scheduleId dan path audio harus ada
            if (string.IsNullOrEmpty(_scheduleId))
            {
                MessageBox.Show("Tidak ada jadwal yang dipilih. Buka form ini dari card jadwal.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(audioPath) || !File.Exists(audioPath))
            {
                MessageBox.Show("File audio tidak ditemukan di: " + audioPath, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] audioBytes = File.ReadAllBytes(audioPath);

                // Multipart/form-data sesuai yang diharapkan TranscriptController
                using var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(_scheduleId), "scheduleId");
                formData.Add(new ByteArrayContent(audioBytes), "AudioFile",
                    Path.GetFileName(audioPath));

                var response = await ApiClient.Http.PostAsync(
                    $"{ApiClient.BaseUrl}/api/transcript/generate", formData);

                if (!response.IsSuccessStatusCode)
                {
                    // Ekstrak pesan error dari body JSON backend jika ada
                    string errBody = await response.Content.ReadAsStringAsync();
                    string errMsg = $"HTTP {(int)response.StatusCode}";

                    if (!string.IsNullOrWhiteSpace(errBody))
                    {
                        try
                        {
                            using var errDoc = JsonDocument.Parse(errBody);
                            if (errDoc.RootElement.TryGetProperty("message", out var mp))
                                errMsg = mp.GetString() ?? errMsg;
                        }
                        catch { errMsg += "\n" + errBody; /* body bukan JSON */ }
                    }

                    MessageBox.Show("Gagal membuat transcript:\n" + errMsg, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Transcript berhasil dibuat! Memuat...", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadBothTranscripts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat upload audio: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mengambil kedua versi transcript dari backend dan menampilkannya
        // txtDraft = versi sederhana (Patient-friendly)
        // txtFinal = versi teknis klinis (Psikolog)
        private async Task LoadBothTranscripts()
        {
            try
            {
                var simpleResp = await ApiClient.Http.GetAsync(
                    $"{ApiClient.BaseUrl}/api/transcript/{_scheduleId}?role=Patient");

                var techResp = await ApiClient.Http.GetAsync(
                    $"{ApiClient.BaseUrl}/api/transcript/{_scheduleId}?role=Psikolog");

                if (simpleResp.IsSuccessStatusCode)
                {
                    string json = await simpleResp.Content.ReadAsStringAsync();

                    // Guard: body kosong tidak bisa di-parse — ini sumber utama "no JSON tokens"
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        using var doc = JsonDocument.Parse(json);
                        // TryGetProperty: tidak lempar exception jika properti tidak ada
                        if (doc.RootElement.TryGetProperty("transcript", out var tp))
                            txtDraft.Text = tp.GetString() ?? "";
                    }
                }

                if (techResp.IsSuccessStatusCode)
                {
                    string json = await techResp.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        using var doc = JsonDocument.Parse(json);
                        if (doc.RootElement.TryGetProperty("transcript", out var tp))
                            txtFinal.Text = tp.GetString() ?? "";
                    }
                }

                // Tampilkan pesan jika kedua field masih kosong setelah fetch
                if (string.IsNullOrEmpty(txtDraft.Text) && string.IsNullOrEmpty(txtFinal.Text))
                    MessageBox.Show("Transcript belum tersedia untuk jadwal ini.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat transcript: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---- TOMBOL BACK ----
        private void buttonBack_Click(object sender, EventArgs e)
        {
            // Hentikan rekaman jika masih berjalan (tidak throw jika null)
            waveIn?.StopRecording();
            this.Close();
        }
    }
}
