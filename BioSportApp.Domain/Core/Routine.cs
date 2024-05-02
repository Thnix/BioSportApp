using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain.Core
{
    public class Routine
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;        
        public DateTime CreationDate { get; set; }

        [ForeignKey(typeof(User))]
        public Guid UserId { get; set; }

        [ManyToOne]
        public User User { get; set; } = null!;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<RoutineExercise> RoutineExercises { get; set; } = [];
    }
}
