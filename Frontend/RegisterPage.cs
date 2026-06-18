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

        private void label3_Click(object sender, EventArgs e) { }

        // Tombol "Sudah punya akun? Login" — kembali ke halaman login
        private void button1_Click(object sender, EventArgs e)
        {
            loginpage halamanLogin = new loginpage();
            halamanLogin.Show();
            this.Hide();
        }

        private void RegisterPage_Load(object sender, EventArgs e) { }

        private async void btnDaftar_Click(object sender, EventArgs e)
        {
            // Validasi: password dan konfirmasi harus cocok
            if (txtPasswordReg.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Konfirmasi password tidak cocok!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bungkus data registrasi (tambah nilai default untuk field wajib Patient)
            var registerData = new
            {
                Name = txtNama.Text,
                Email = txtUsernameReg.Text,
                Password = txtPasswordReg.Text,
                BirthDate = DateTime.Now.ToString("yyyy-MM-dd"),
                phoneNumber = "081234567890",
                Gender = "Pria",
                role = "Patient"
            };

            string json = JsonSerializer.Serialize(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Kirim POST ke endpoint register menggunakan ApiClient bersama (SSL bypass)
                HttpResponseMessage response = await ApiClient.Http.PostAsync(
                    $"{ApiClient.BaseUrl}/api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Registrasi Berhasil! Silakan Login.", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Setelah berhasil, kembali ke halaman login
                    loginpage halamanLogin = new loginpage();
                    this.Hide();
                    halamanLogin.ShowDialog();
                    this.Close();
                }
                else
                {
                    // Tampilkan pesan error dari server
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gagal mendaftar! Balasan server:\n" + errorResponse,
                        "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke server: " + ex.Message, "Error");
            }
        }
    }
}
