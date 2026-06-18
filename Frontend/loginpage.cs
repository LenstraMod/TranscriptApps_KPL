using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Frontend
{
    public partial class loginpage : Form
    {
        public loginpage()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void loginpage_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDftr_Click(object sender, EventArgs e)
        {
            RegisterPage halamanRegister = new RegisterPage();
            halamanRegister.Show();
            this.Hide();
        }

        private async void btnMasuk_Click(object sender, EventArgs e)
        {
            // 1. Bungkus data dari textbox ke dalam sebuah objek (sesuai LoginRequest di backend)
            var loginData = new
            {
                Email = txtUsername.Text,
                Password = txtPassword.Text
            };
            // 2. Ubah data tersebut menjadi format JSON
            string json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // 3. Kirim ke Backend menggunakan HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Pastikan port 7199 sesuai dengan terminal backend-mu
                    HttpResponseMessage response = await client.PostAsync("https://localhost:7199/api/auth/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Login Berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Pindah ke Home Page
                        HomePage halamanHome = new HomePage();
                        halamanHome.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Email atau password salah!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
