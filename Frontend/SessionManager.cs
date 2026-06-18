namespace Frontend
{
    // Menyimpan data user yang sedang login secara global agar bisa diakses dari semua form
    // Diisi saat login berhasil dan dipakai oleh Home, FormInputJadwal, Transkrip_UI
    public static class SessionManager
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; } = "";
        public static string UserRole { get; set; } = ""; // "Patient" atau "Psikolog"
        public static string UserEmail { get; set; } = "";
    }
}
