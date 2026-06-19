using System.Text.Json;

namespace JsonHelperLibrary
{
    public class JsonHelper
    {
        private string _dataFolder;

        // Lock objek per-instance untuk mencegah dua operasi SaveJson terjadi bersamaan
        // dalam proses yang sama (misal: dua request HTTP masuk sekaligus)
        private readonly object _saveLock = new object();

        public JsonHelper(string config) {
            _dataFolder = config;
        }

        //Implementasi Generics

        public List<T> LoadJson<T>(string filename) {
            string path = Path.Combine(_dataFolder, filename);

            if (!File.Exists(path)) {
                return new List<T>();
            }

            string json = File.ReadAllText(path);

            // Guard: file kosong (misalnya karena write sebelumnya terganggu)
            // JsonSerializer.Deserialize("") akan lempar "no JSON tokens" tanpa guard ini
            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            // PropertyNameCaseInsensitive: supaya "id" di JSON bisa masuk ke properti "Id" di C#
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
        }

        public void SaveJson<T>(string filename, List<T> data)
        {
            string path = Path.Combine(_dataFolder, filename);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions {
                WriteIndented = true,
            });

            lock (_saveLock)
            {
                // Tulis ke file sementara dulu, baru ganti file aslinya secara atomik.
                // Tujuan: mencegah IOException "file locked by another process" dari file watcher
                // atau proses lain yang membaca file asli saat kita sedang menulis.
                string tempPath = path + ".tmp";

                // Coba simpan, ulangi sampai 5 kali jika file sedang terkunci sementara
                for (int attempt = 0; attempt < 5; attempt++)
                {
                    try
                    {
                        File.WriteAllText(tempPath, json);
                        // File.Move dengan overwrite: true bersifat atomik di Windows —
                        // pembaca selalu mendapat file yang utuh, tidak pernah file setengah jadi
                        File.Move(tempPath, path, overwrite: true);
                        return;
                    }
                    catch (IOException) when (attempt < 4)
                    {
                        // Tunggu sebentar lalu coba lagi (jeda bertambah tiap percobaan)
                        Thread.Sleep(50 * (attempt + 1));
                    }
                }
            }
        }
    }
}
