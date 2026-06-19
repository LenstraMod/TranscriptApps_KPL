using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Frontend
{
    public partial class FormInputJadwal : Form
    {
        // Variabel penampung data jadwal yang akan dikembalikan ke Home
        public JadwalModel DataJadwal = new JadwalModel();

        // Jika tidak null, form berjalan dalam mode edit
        private readonly string? _editScheduleId;
        private readonly SessionDto? _existingSession;

        // Constructor untuk mode tambah (default)
        public FormInputJadwal()
        {
            InitializeComponent();
            btnSave.Click += new System.EventHandler(this.btnSave_Click);
            btnPilihGambar.Click += new System.EventHandler(this.btnPilihGambar_Click);
        }

        // Constructor untuk mode edit — pre-fill data sesi yang ada
        public FormInputJadwal(string scheduleId, SessionDto existingSession)
        {
            InitializeComponent();
            btnSave.Click += new System.EventHandler(this.btnSave_Click);
            btnPilihGambar.Click += new System.EventHandler(this.btnPilihGambar_Click);

            _editScheduleId = scheduleId;
            _existingSession = existingSession;
        }

        private void FormInputJadwal_Load(object sender, EventArgs e)
        {
            txtHari.ReadOnly = true;

            if (cmbJenisSesi.Items.Count > 0)
                cmbJenisSesi.SelectedIndex = 0;

            // Jika yang login adalah Psikolog, otomatis isi nama psikolog dan kunci fieldnya
            if (SessionManager.UserRole == "Psikolog")
            {
                txtNamaPsikolog.Text = SessionManager.UserName;
                txtNamaPsikolog.ReadOnly = true;
            }

            // Mode edit: pre-fill data sesi yang sudah ada
            if (_editScheduleId != null && _existingSession != null)
            {
                this.Text = "Edit Jadwal";
                btnPilihGambar.Visible = false;
                lblPathGambar.Visible = false;

                if (DateTime.TryParse(_existingSession.Date, out var dt))
                    dtpTanggal.Value = dt;

                txtHari.Text = _existingSession.Day;
                txtMulai.Text = _existingSession.StartTime;
                txtAkhir.Text = _existingSession.EndTime;

                // Pilih item ComboBox yang sesuai dengan metode yang ada
                for (int i = 0; i < cmbJenisSesi.Items.Count; i++)
                {
                    if (string.Equals(cmbJenisSesi.Items[i]?.ToString(),
                            _existingSession.Method, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbJenisSesi.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        // Auto-isi kolom hari ketika tanggal dipilih
        private void dtpTanggal_ValueChanged(object sender, EventArgs e)
        {
            txtHari.Text = dtpTanggal.Value.DayOfWeek.ToString();
        }

        // Buka dialog untuk memilih gambar psikolog
        private void btnPilihGambar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    lblPathGambar.Text = System.IO.Path.GetFileName(ofd.FileName);
                    DataJadwal.PathGambar = ofd.FileName;
                }
            }
        }

        // Dipanggil saat tombol Simpan diklik — validasi data lalu kirim ke backend
        private async void btnSave_Click(object sender, EventArgs e)
        {
            // Kumpulkan data dari form ke dalam JadwalModel
            DataJadwal.NamaPsikolog = txtNamaPsikolog.Text;
            DataJadwal.TanggalSesi = dtpTanggal.Value.ToString("yyyy-MM-dd");
            DataJadwal.HariSesi = txtHari.Text;
            DataJadwal.JamMulai = txtMulai.Text;
            DataJadwal.JamAkhir = txtAkhir.Text;

            if (cmbJenisSesi.SelectedItem != null)
                DataJadwal.JenisSesi = cmbJenisSesi.SelectedItem.ToString() ?? "";

            try
            {
                HttpResponseMessage response;

                if (_editScheduleId != null)
                {
                    // Mode edit: kirim hanya data sesi via PUT
                    var sessionPayload = new
                    {
                        date = DataJadwal.TanggalSesi,
                        day = DataJadwal.HariSesi,
                        startTime = DataJadwal.JamMulai,
                        endTime = DataJadwal.JamAkhir,
                        method = DataJadwal.JenisSesi,
                        meetingLink = (string?)null,
                        location = (string?)null
                    };

                    string json = JsonSerializer.Serialize(sessionPayload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await ApiClient.Http.PutAsync(
                        $"{ApiClient.BaseUrl}/api/schedule/{_editScheduleId}", content);
                }
                else
                {
                    // Mode tambah: kirim seluruh objek jadwal via POST
                    var schedulePayload = new
                    {
                        scheduleId = "",      // backend akan generate ID ini secara otomatis
                        psychologist = new
                        {
                            psychologistId = SessionManager.UserId,
                            name = SessionManager.UserName,
                            email = SessionManager.UserEmail,
                            strNumber = "",   // tidak tersedia dari session, backend tidak validasi
                            experience = 0
                        },
                        session = new
                        {
                            date = DataJadwal.TanggalSesi,
                            day = DataJadwal.HariSesi,
                            startTime = DataJadwal.JamMulai,
                            endTime = DataJadwal.JamAkhir,
                            method = DataJadwal.JenisSesi,
                            meetingLink = (string?)null,
                            location = (string?)null
                        },
                        status = "Tersedia",
                        bookedBy = (object?)null
                    };

                    string json = JsonSerializer.Serialize(schedulePayload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await ApiClient.Http.PostAsync(
                        $"{ApiClient.BaseUrl}/api/schedule/add", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    string msg = _editScheduleId != null
                        ? "Jadwal berhasil diupdate!"
                        : "Jadwal berhasil ditambahkan!";
                    MessageBox.Show(msg, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    string err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gagal menyimpan jadwal:\n" + err, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke server: " + ex.Message, "Error");
            }
        }
    }

    // Model data jadwal lokal — dipakai sebagai perantara data form
    public class JadwalModel
    {
        public string NamaPsikolog { get; set; } = "";
        public string TanggalSesi { get; set; } = "";
        public string HariSesi { get; set; } = "";
        public string JamMulai { get; set; } = "";
        public string JamAkhir { get; set; } = "";
        public string JenisSesi { get; set; } = "";
        public string PathGambar { get; set; } = "";
    }
}
