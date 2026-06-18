using System.Net.Http;

namespace Frontend
{
    // Menyediakan satu HttpClient bersama yang sudah dikonfigurasi untuk seluruh aplikasi
    // Menggunakan SSL bypass karena backend menggunakan sertifikat self-signed di localhost
    public static class ApiClient
    {
        // URL dasar backend — sesuaikan port jika berbeda
        public const string BaseUrl = "https://localhost:7199";

        // HttpClient tunggal dengan handler yang melewati validasi SSL localhost
        public static readonly HttpClient Http = new HttpClient(new HttpClientHandler
        {
            // Bypass certificate validation khusus untuk development localhost
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });
    }
}
