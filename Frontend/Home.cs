using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Frontend
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FormInputJadwal popUpInput = new FormInputJadwal())
            {
                if (popUpInput.ShowDialog() == DialogResult.OK)
                {
                    string nama = popUpInput.DataJadwal.NamaPsikolog;
                    string tanggal = popUpInput.DataJadwal.TanggalSesi;
                    string hari = popUpInput.DataJadwal.HariSesi;
                    string mulai = popUpInput.DataJadwal.JamMulai;
                    string akhir = popUpInput.DataJadwal.JamAkhir;
                    string jenis = popUpInput.DataJadwal.JenisSesi;
                    string pathGambar = popUpInput.DataJadwal.PathGambar; // <--- Tangkap lokasi gambarnya

                    // BARIS INI WAJIB ADA: Buat cetakan card baru dari UserControl1
                    UserControl1 cardBaru = new UserControl1();

                    // MASUKKAN DATA: Tembak fungsi SetCardData yang ada di UserControl1 kemarin
                    cardBaru.SetCardData(nama, tanggal, hari, mulai, akhir, jenis, pathGambar);

                    // Masukkan fisik cardBaru yang sudah jadi ke dalam panel utama
                    flowLayoutPanel1.Controls.Add(cardBaru);
                }
            }
        }

        private void Details_Click(object sender, EventArgs e)
        {

        }

    }
}
