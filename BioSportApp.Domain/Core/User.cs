using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain.Core
{
    public class User
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string FirstSurname { get; set; } = null!;
        public string SecondSurname { get; set; } = null!;
        public string Nip { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public int Code { get; set; }
        public DateTime BirthdayDate { get; set; }
        public byte[]? Photo { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Routine> Routines { get; set; } = [];
    }
}
