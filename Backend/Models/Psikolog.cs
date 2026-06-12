namespace Backend.Models
{
    public class Psikolog : User
    {
        public string STRNumber { get; set;}
        public int Experience { get; set;}
        public List<string> AvailableScedule { get; set; }

    }
}
