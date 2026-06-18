using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;

namespace Frontend
{
    public partial class Transkrip_UI : Form
    {
        // Variabel untuk NAudio (Perekaman)
        private WaveInEvent waveIn;
        private WaveFileWriter waveWriter;
        private string outputAudioPath;

        public Transkrip_UI()
        {
            InitializeComponent();

            // Memastikan kotak Draft dan Final dalam keadaan kosong saat dimuat
            txtDraft.Clear();
            txtFinal.Clear();
        }

        // 1. Fungsi saat tombol START diklik
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Tentukan jalur folder 'audio' di dalam folder Debug/Release aplikasi
                string folderAudio = Path.Combine(Application.StartupPath, "audio");

                // Jika folder 'audio' belum ada, buat foldernya secara otomatis
                if (!Directory.Exists(folderAudio))
                {
                    Directory.CreateDirectory(folderAudio);
                }

                // Tentukan lokasi file audio di dalam folder baru tersebut
                string namaFile = $"rekaman_{DateTime.Now:yyyyMMdd_HHmmss}.wav"; // Menggunakan timestamp agar file tidak menimpa yang lama
                outputAudioPath = Path.Combine(folderAudio, namaFile);

                // Konfigurasi NAudio
                waveIn = new WaveInEvent();
                waveIn.WaveFormat = new WaveFormat(44100, 1); // Mono, 44.1kHz

                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.RecordingStopped += WaveIn_RecordingStopped;

                waveWriter = new WaveFileWriter(outputAudioPath, waveIn.WaveFormat);

                // Mulai merekam
                waveIn.StartRecording();

                // Pop-up notifikasi kecil indikator bahwa mic aktif sedang merekam
                MessageBox.Show("Merekam... Silakan berbicara.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memulai perekaman: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Menulis data suara dari mic ke file .wav
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveWriter != null)
            {
                waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
                waveWriter.Flush();
            }
        }

        // 2. Fungsi saat tombol STOP diklik
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                waveIn.StopRecording(); // Menghentikan perekaman mikrofon
            }
        }

        // Event yang dipicu otomatis saat perekaman berhenti
        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            // Tutup dan lepaskan resource file writer
            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }

            if (waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }

            // Berikan pop-up sukses beserta lokasi folder barunya
            MessageBox.Show($"Perekaman selesai!\n\nFile disimpan di folder khusus:\n{outputAudioPath}",
                            "Perekaman Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Tombol Back untuk kembali ke HomePage
        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
            }

            this.Hide();
            HomePage halamanHome = new HomePage();
            halamanHome.ShowDialog();
            this.Close();
        }
    }
}