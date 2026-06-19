using Backend.Models;
using JsonHelperLibrary;
using Microsoft.OpenApi;

namespace Backend.Services
{
    //Service yang menangani autentikasi login dan register
    public class AuthService
    {
        private JsonHelper _jsonHelper;
        private List<Psikolog> _psikologList;
        private List<Patient> _patientList;

        public AuthService(JsonHelper jsonHelper) 
        {
            _jsonHelper = jsonHelper;
            _psikologList = _jsonHelper.LoadJson<Psikolog>("Psikolog.json");
            _patientList = _jsonHelper.LoadJson<Patient>("Patient.json");
        }

        //fungsi untuk login. Disini validasi dari login yang dilakukan
        public User? Login(string email, string password)
        {
            // Preconditions
            Contract.Requires(!string.IsNullOrWhiteSpace(email), "Email tidak boleh kosong");
            Contract.Requires(!string.IsNullOrWhiteSpace(password), "Password tidak boleh kosong");

            //Cari dan validasi data psikolog. Jika ada psikolog maka akan di return
            var psikolog = _psikologList.FirstOrDefault(p =>
            p.email == email &&
            p.password == password
            );
            if (psikolog != null) return psikolog;

            //Cari dan validasi data pasien. Jika ada pasien maka akan di return
            var patient = _patientList.FirstOrDefault(p =>
            p.email == email &&
            p.password == password
            );
            if(patient != null) return patient;

            return null;
        }

        //Fungsi untuk register. Menambahkan user baru(hanya pasien saja) ke dalam list
        public bool Register(Patient patient) {
            // Preconditions
            Contract.Requires(patient != null, "Data patient tidak boleh null");
            Contract.Requires(!string.IsNullOrWhiteSpace(patient!.email), "Email patient tidak boleh kosong");
            Contract.Requires(!string.IsNullOrWhiteSpace(patient.password), "Password patient tidak boleh kosong");

            //Melaukan defense jika email sudah ada maka return false
            bool emailExists =
                _psikologList.Any(p => p.email == patient.email) ||
                _patientList.Any(p => p.email == patient.email);

            if (emailExists) return false;

            //Jika tidak ada maka lanjut membuat user baru dan simpan ke dalam list patient dan simpan ke json
            patient.Id = GetNextId();
            patient.role = "Patient";

            _patientList.Add(patient);
            _jsonHelper.SaveJson("Patient.json", _patientList);

            // Postcondition: patient benar-benar masuk ke list
            Contract.Ensures(_patientList.Any(p => p.Id == patient.Id),
                "Patient baru harus ada di list setelah Register");

            return true;
        }

        //Ambil nextID dari masing masing list
        private int GetNextId()
        {
            //Lakukan cek apakah ada data pada list masing masing. Jika ada maka ambil index maxnya
            int maxPsikolog = _psikologList.Any() ? _psikologList.Max(p => p.Id) : 0;
            int maxPatient = _patientList.Any() ? _patientList.Max(p => p.Id) : 0;

            //Mengembalikan nilai id yang paling tinggi
            return Math.Max(maxPsikolog, maxPatient) + 1;
        }

        //Mendapatkan user GetByID
        public User? GetById(int id) 
        {
            User? user = _psikologList.FirstOrDefault(p => p.Id == id);
            return user ?? _patientList.FirstOrDefault(p => p.Id == id);
        }
    }
}
