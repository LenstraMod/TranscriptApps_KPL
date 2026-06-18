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

        public FormInputJadwal()
        {
            InitializeComponent();

            // Daftarkan event handler secara manual (bukan lewat designer)
            btnSave.Click += new System.EventHandler(this.btnSave_Click);
            btnPilihGambar.Click += new System.EventHandler(this.btnPilihGambar_Click);
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

            // Bangun objek Schedule sesuai format yang diharapkan backend
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
                status = "Tersedia",  // jadwal baru selalu mulai dengan status Tersedia
                bookedBy = (object?)null
            };

            string json = JsonSerializer.Serialize(schedulePayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // POST jadwal baru ke backend — backend menyimpannya ke data/schedule.json
                HttpResponseMessage response = await ApiClient.Http.PostAsync(
                    $"{ApiClient.BaseUrl}/api/schedule/add", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Jadwal berhasil ditambahkan!", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tutup form dan sinyal ke Home bahwa penyimpanan berhasil
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Tampilkan pesan error dari server
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
