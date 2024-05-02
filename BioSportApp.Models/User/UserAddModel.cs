namespace BioSportApp.Models.User
{
    public class UserAddModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstSurname { get; set; } = string.Empty;
        public string SecondSurname { get; set; } = string.Empty;
        public string Nip { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Code { get; set; }
        public DateTime BirthdayDate { get; set; }
        public byte[]? Photo { get; set; }
    }
}
