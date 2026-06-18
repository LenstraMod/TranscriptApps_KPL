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

        private void label1_Click(object sender, EventArgs e) { }

        private void loginpage_Load(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void btnDftr_Click(object sender, EventArgs e)
        {
            RegisterPage halamanRegister = new RegisterPage();
            halamanRegister.Show();
            this.Hide();
        }

        private async void btnMasuk_Click(object sender, EventArgs e)
        {
            var loginData = new
            {
                Email = txtUsername.Text,
                Password = txtPassword.Text
            };

            string json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await ApiClient.Http.PostAsync(
                    $"{ApiClient.BaseUrl}/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();

                    // Guard: body kosong tidak bisa di-parse — terjadi jika server error tanpa body
                    if (string.IsNullOrWhiteSpace(responseJson))
                    {
                        MessageBox.Show("Server mengembalikan respons kosong.", "Error");
                        return;
                    }

                    using var doc = JsonDocument.Parse(responseJson);
                    var root = doc.RootElement;

                    // TryGetProperty lebih aman dari GetProperty — tidak lempar exception
                    // jika backend berubah dan properti tidak ada
                    SessionManager.UserId = root.TryGetProperty("id", out var idProp)
                        ? idProp.GetInt32() : 0;
                    SessionManager.UserName = root.TryGetProperty("name", out var nameProp)
                        ? nameProp.GetString() ?? "" : "";
                    SessionManager.UserRole = root.TryGetProperty("role", out var roleProp)
                        ? roleProp.GetString() ?? "" : "";
                    SessionManager.UserEmail = root.TryGetProperty("email", out var emailProp)
                        ? emailProp.GetString() ?? "" : "";

                    MessageBox.Show(
                        $"Login Berhasil! Selamat datang, {SessionManager.UserName}.",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Home halamanHome = new Home();
                    halamanHome.Show();
                    this.Hide();
                }
                else
                {
                    // Coba tampilkan pesan dari body server jika ada
                    string errBody = await response.Content.ReadAsStringAsync();
                    string pesan = "Email atau password salah!";

                    if (!string.IsNullOrWhiteSpace(errBody))
                    {
                        try
                        {
                            using var errDoc = JsonDocument.Parse(errBody);
                            if (errDoc.RootElement.TryGetProperty("message", out var msgProp))
                                pesan = msgProp.GetString() ?? pesan;
                        }
                        catch { /* body bukan JSON, pakai pesan default */ }
                    }

                    MessageBox.Show(pesan, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke server: " + ex.Message, "Error");
            }
        }
    }
}
