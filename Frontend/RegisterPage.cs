using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;

namespace Frontend
{
    public partial class RegisterPage : Form
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            loginpage halamanLogin = new loginpage();
            halamanLogin.Show();
            this.Hide();
        }

        private void RegisterPage_Load(object sender, EventArgs e)
        {

        }

        private async void btnDaftar_Click(object sender, EventArgs e)
        {
            // 1. Cek apakah password dan konfirmasi sama
            if (txtPasswordReg.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Konfirmasi password tidak cocok!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Bungkus data asli + data dummy untuk memenuhi syarat class Patient
            var registerData = new
            {
                Name = txtNama.Text,
                Email = txtUsernameReg.Text,
                Password = txtPasswordReg.Text,
                BirthDate = DateTime.Now.ToString("yyyy-MM-dd"),
                phoneNumber = "081234567890",
                Gender = "Pria",
                role = "Patient" // <-- TAMBAHKAN BARIS INI
            };

            // 3. Ubah data menjadi JSON agar bisa dibaca Backend
            string json = JsonSerializer.Serialize(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 4. Kirim ke Backend
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Pastikan URL ini pakai port yang muncul di terminal hitammu (7199 atau 5148)
                    HttpResponseMessage response = await client.PostAsync("https://localhost:7199/api/auth/register", content);

                    // 5. Jika backend merespons OK (Sukses masuk database)
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Registrasi Berhasil! Silakan Login.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Transisi ke Halaman Login dengan mulus
                        loginpage halamanLogin = new loginpage();
                        this.Hide();
                        halamanLogin.ShowDialog();
                        this.Close();
                    }
                    // ... kode try dan if (response.IsSuccessStatusCode) sebelumnya ...
                    else
                    {
                        // Membaca alasan penolakan asli dari server
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Gagal mendaftar! Balasan server:\n" + errorResponse, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal terhubung ke server: " + ex.Message, "Error");
                }
            }
        }
    }
}
