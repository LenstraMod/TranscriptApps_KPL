using System.Text.Json;

namespace JsonHelperLibrary
{
    public class JsonHelper
    {
        private string _dataFolder;

        public JsonHelper(string config) {
            _dataFolder = config;
        }

        public List<T> LoadJson<T>(string filename) {
            string path = Path.Combine(_dataFolder, filename);

            if (!File.Exists(path)) {
                return new List<T>();
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void SaveJson<T>(string filename, List<T> data) 
        {
            string path = Path.Combine(_dataFolder, filename);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions {
                WriteIndented = true,
            });

            File.WriteAllText(path, json);
        }
    }
}
