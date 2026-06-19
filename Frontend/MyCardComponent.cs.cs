using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;

namespace Frontend
{
    public partial class UserControl1 : UserControl
    {
        // Menyimpan ID jadwal yang ditampilkan oleh card ini
        public string ScheduleId { get; private set; } = "";

        // Event yang dilempar ke Home agar list di-refresh setelah delete/edit
        public event EventHandler? CardDeleted;
        public event EventHandler? CardEdited;

        // Data sesi lokal — dipakai untuk pre-fill form edit
        private string _sessionDate = "";
        private string _sessionDay = "";
        private string _sessionStartTime = "";
        private string _sessionEndTime = "";
        private string _sessionMethod = "";

        public UserControl1()
        {
            InitializeComponent();
        }

        // Mengisi data card dari luar — dipanggil oleh Home.cs setelah card dibuat
        public void SetCardData(string nama, string tanggal, string hari,
            string mulai, string akhir, string jenis, string pathGambar, string scheduleId = "")
        {
            ScheduleId = scheduleId;

            // Simpan data sesi untuk keperluan edit
            _sessionDate = tanggal;
            _sessionDay = hari;
            _sessionStartTime = mulai;
            _sessionEndTime = akhir;
            _sessionMethod = jenis;

            label1.Text = nama;
            label2.Text = tanggal;
            label3.Text = hari;
            label4.Text = mulai;
            label5.Text = akhir;
            label6.Text = jenis;

            // Tampilkan gambar jika path valid; kosongkan pictureBox jika tidak ada
            if (!string.IsNullOrEmpty(pathGambar) && System.IO.File.Exists(pathGambar))
                pictureBox1.Image = Image.FromFile(pathGambar);
            else
                pictureBox1.Image = null;

            // Tombol Edit dan Hapus hanya untuk Psikolog
            bool isPsikolog = SessionManager.UserRole == "Psikolog";
            btnEdit.Visible = isPsikolog;
            btnDelete.Visible = isPsikolog;
        }

        // Dipanggil saat tombol "Details" diklik
        private void Details_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ScheduleId))
            {
                MessageBox.Show("ID jadwal tidak ditemukan.", "Info");
                return;
            }

            var transkripForm = new Transkrip_UI(ScheduleId, SessionManager.UserRole);
            transkripForm.ShowDialog();
        }

        // Dipanggil saat gambar psikolog diklik — sama dengan klik Details
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Details_Click(sender, e);
        }

        // Buka FormInputJadwal dalam mode edit dengan data sesi yang sudah ada
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ScheduleId)) return;

            var existingSession = new SessionDto
            {
                Date = _sessionDate,
                Day = _sessionDay,
                StartTime = _sessionStartTime,
                EndTime = _sessionEndTime,
                Method = _sessionMethod
            };

            using var form = new FormInputJadwal(ScheduleId, existingSession);
            if (form.ShowDialog() == DialogResult.OK)
                CardEdited?.Invoke(this, EventArgs.Empty);
        }

        // Konfirmasi lalu kirim DELETE ke backend
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ScheduleId)) return;

            var confirm = MessageBox.Show(
                "Yakin ingin menghapus jadwal ini?", "Konfirmasi Hapus",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                HttpResponseMessage response = await ApiClient.Http.DeleteAsync(
                    $"{ApiClient.BaseUrl}/api/schedule/{ScheduleId}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Jadwal berhasil dihapus.", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CardDeleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    string err = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gagal menghapus jadwal:\n" + err, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke server: " + ex.Message, "Error");
            }
        }
    }
}
