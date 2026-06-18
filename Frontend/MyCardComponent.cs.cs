using System;
using System.Drawing;
using System.Windows.Forms;

namespace Frontend
{
    public partial class UserControl1 : UserControl
    {
        // Menyimpan ID jadwal yang ditampilkan oleh card ini
        // Dipakai saat tombol Details diklik untuk membuka Transkrip_UI dengan jadwal yang benar
        public string ScheduleId { get; private set; } = "";

        public UserControl1()
        {
            InitializeComponent();
        }

        // Mengisi data card dari luar — dipanggil oleh Home.cs setelah card dibuat
        // scheduleId opsional agar kompatibel dengan panggilan lama (tanpa ID)
        public void SetCardData(string nama, string tanggal, string hari,
            string mulai, string akhir, string jenis, string pathGambar, string scheduleId = "")
        {
            // Simpan ID jadwal agar bisa dipakai saat tombol Details diklik
            ScheduleId = scheduleId;

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
        }

        // Dipanggil saat tombol "Details" di dalam card diklik
        // Membuka halaman Transkrip_UI dengan ID jadwal milik card ini
        private void Details_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ScheduleId))
            {
                MessageBox.Show("ID jadwal tidak ditemukan.", "Info");
                return;
            }

            // Buka Transkrip_UI sebagai dialog modal — form Home tetap aktif di belakang
            var transkripForm = new Transkrip_UI(ScheduleId, SessionManager.UserRole);
            transkripForm.ShowDialog();
        }

        // Dipanggil saat gambar psikolog diklik — sama dengan klik Details
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Details_Click(sender, e);
        }
    }
}
