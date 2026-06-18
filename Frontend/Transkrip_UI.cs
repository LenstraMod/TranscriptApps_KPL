using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Frontend
{
    public partial class Transkrip_UI : Form
    {
        // 1. Data dummy transkrip
        private string[] kataDraft = {
            "Halo selamat pagi semuanya, ",
            "hari ini kita akan membahas mengenai progres ",
            "pengembangan aplikasi transkrip berbasis GUI. ",
            "Target kita minggu ini adalah menyelesaikan frontend ",
            "dan mengintegrasikannya dengan backend sistem."
        };

        private string teksFinal = "Inti Rapat: Pembahasan progres aplikasi transkrip GUI. Target minggu ini menyelesaikan frontend dan integrasi backend.";

        private int indeksKata = 0;
        private System.Windows.Forms.Timer timerTranskrip;

        public Transkrip_UI()
        {
            InitializeComponent();
            SetupTimer();
        }

        // 2. Inisialisasi Timer
        private void SetupTimer()
        {
            timerTranskrip = new System.Windows.Forms.Timer();
            timerTranskrip.Interval = 1500;
            timerTranskrip.Tick += TimerTranskrip_Tick;
        }

        // 3. Fungsi saat tombol START diklik
        private void button1_Click(object sender, EventArgs e)
        {
            txtDraft.Clear();
            txtFinal.Clear();
            indeksKata = 0;
            timerTranskrip.Start();
        }

        // 4. Fungsi saat tombol STOP diklik
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (timerTranskrip.Enabled)
            {
                timerTranskrip.Stop();
                txtFinal.Text = teksFinal;
            }
        }

        // 5. Fungsi BARU saat tombol BACK diklik
        private void buttonBack_Click(object sender, EventArgs e)
        {
            // Pastikan timer berhenti jika masih berjalan sebelum pindah halaman
            timerTranskrip.Stop();

            // Membuat instansiasi objek halaman HomePage baru
            HomePage halamanHome = new HomePage();

            // Memunculkan halaman HomePage
            halamanHome.Show();

            // Menutup/menyembunyikan halaman Transkrip_UI saat ini
            this.Close();
        }

        // 6. Efek simulasi real-time mengetik kata demi kata
        private void TimerTranskrip_Tick(object sender, EventArgs e)
        {
            if (indeksKata < kataDraft.Length)
            {
                txtDraft.AppendText(kataDraft[indeksKata]);
                indeksKata++;
            }
            else
            {
                timerTranskrip.Stop();
                txtFinal.Text = teksFinal;
            }
        }
    }
}