using Backend.Models;
using JsonHelperLibrary;
using Microsoft.OpenApi;

namespace Backend.Services
{
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

        public User? Login(string email, string password)
        { 
            var psikolog = _psikologList.FirstOrDefault(p => 
            p.email == email &&
            p.password == password
            );
            if (psikolog != null) return psikolog; 

            var patient = _patientList.FirstOrDefault(p =>
            p.email == email &&
            p.password == password
            );
            if(patient != null) return patient;

            return null;
        }

        public bool Register(Patient patient) { 
            
            bool emailExists =
                _psikologList.Any(p => p.email == patient.email) ||
                _patientList.Any(p => p.email == patient.email);

            if (emailExists) return false;

            patient.Id = GetNextId();
            patient.role = "Patient";

            _patientList.Add(patient);
            _jsonHelper.SaveJson("Patient.json", _patientList);

            return true;
        }

        private int GetNextId()
        { 
            int maxPsikolog = _psikologList.Any() ? _psikologList.Max(p => p.Id) : 0;
            int maxPatient = _patientList.Any() ? _psikologList.Max(p => p.Id) : 0;

            return Math.Max(maxPsikolog, maxPatient) + 1;
        }

        public User? GetById(int id) 
        {
            User? user = _psikologList.FirstOrDefault(p => p.Id == id);
            return user ?? _patientList.FirstOrDefault(p => p.Id == id);
        }
    }
}
