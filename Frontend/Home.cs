using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;

namespace Frontend
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            // Sambungkan event Load ke handler agar jadwal langsung dimuat saat form dibuka
            this.Load += Home_Load;
        }

        // Dipanggil otomatis ketika form selesai diinisialisasi
        private async void Home_Load(object sender, EventArgs e)
        {
            // Ubah judul window untuk menampilkan nama user yang sedang login
            this.Text = $"Home — {SessionManager.UserName} ({SessionManager.UserRole})";

            // Tombol tambah jadwal (plusSession) hanya tampil untuk Psikolog
            // Patient tidak bisa menambah jadwal baru — mereka hanya melihat/booking
            plusSession.Visible = (SessionManager.UserRole == "Psikolog");

            // Muat dan tampilkan daftar jadwal dari backend
            await LoadSchedulesFromBackend();
        }

        // Mengambil daftar jadwal dari backend dan menampilkannya sebagai card di flowLayoutPanel1
        private async Task LoadSchedulesFromBackend()
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                string url;
                if (SessionManager.UserRole == "Psikolog")
                {
                    // Psikolog hanya melihat jadwal yang dia miliki sendiri
                    url = $"{ApiClient.BaseUrl}/api/schedule/psikolog/{SessionManager.UserId}";
                }
                else
                {
                    // Patient melihat semua jadwal (bisa filter di sini jika perlu)
                    url = $"{ApiClient.BaseUrl}/api/schedule";
                }

                HttpResponseMessage response = await ApiClient.Http.GetAsync(url);
                if (!response.IsSuccessStatusCode) return;

                string json = await response.Content.ReadAsStringAsync();

                // Guard: body kosong tidak bisa di-parse (terjadi saat backend error tanpa body)
                if (string.IsNullOrWhiteSpace(json)) return;

                // Parse JSON jadwal menggunakan DTO lokal
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var schedules = JsonSerializer.Deserialize<List<ScheduleDto>>(json, options);

                if (schedules == null) return;

                // Buat satu card untuk setiap jadwal yang diterima
                foreach (var s in schedules)
                {
                    UserControl1 card = new UserControl1();
                    card.SetCardData(
                        s.Psychologist?.Name ?? "-",       // nama psikolog
                        s.Session?.Date ?? "-",            // tanggal sesi
                        s.Session?.Day ?? "-",             // hari sesi
                        s.Session?.StartTime ?? "-",       // jam mulai
                        s.Session?.EndTime ?? "-",         // jam selesai
                        s.Session?.Method ?? "-",          // metode (online/offline)
                        "",                                // path gambar (tidak ada dari backend)
                        s.ScheduleId                       // ID jadwal — disimpan di card untuk navigasi
                    );
                    flowLayoutPanel1.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat jadwal: " + ex.Message, "Error");
            }
        }

        // Tombol tambah jadwal baru — hanya muncul untuk Psikolog
        private void button2_Click(object sender, EventArgs e)
        {
            using (FormInputJadwal popUpInput = new FormInputJadwal())
            {
                if (popUpInput.ShowDialog() == DialogResult.OK)
                {
                    // Setelah jadwal berhasil disimpan ke backend, reload semua card
                    _ = LoadSchedulesFromBackend();
                }
            }
        }

        private void Details_Click(object sender, EventArgs e) { }
    }

    // ---- DTO lokal untuk membaca data jadwal dari JSON backend ----
    // Hanya properti yang dipakai di frontend yang perlu didefinisikan

    public class ScheduleDto
    {
        public string ScheduleId { get; set; } = "";
        public PsychologistDto? Psychologist { get; set; }
        public SessionDto? Session { get; set; }
        public string Status { get; set; } = "";
    }

    public class PsychologistDto
    {
        public int PsychologistId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class SessionDto
    {
        public string Date { get; set; } = "";
        public string Day { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public string Method { get; set; } = "";
    }
}
