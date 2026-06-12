namespace Backend.Models
{
    public class Patient : User
    {
        public DateTime BirthDate {get; set;}
        public string phoneNumber {get; set;}
        public string Gender{ get; set;}

    }
}
