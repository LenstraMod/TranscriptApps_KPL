using System;
using System.Drawing;
using System.Windows.Forms;

namespace Frontend
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        // Fungsi ini wajib berada di sini agar bisa membaca pictureBox1 milik card
        public void SetCardData(string nama, string tanggal, string hari, string mulai, string akhir, string jenis, string pathGambar)
        {
            label1.Text = nama;
            label2.Text = tanggal;
            label3.Text = hari;
            label4.Text = mulai;
            label5.Text = akhir;
            label6.Text = jenis;

            if (!string.IsNullOrEmpty(pathGambar) && System.IO.File.Exists(pathGambar))
            {
                pictureBox1.Image = Image.FromFile(pathGambar);
            }
            else
            {
                pictureBox1.Image = Image.FromFile("cukingdipeyuk.png");
            }
        }
    }
}