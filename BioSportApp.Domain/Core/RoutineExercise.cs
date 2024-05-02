using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain.Core
{
    public class RoutineExercise
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public int SetsNumber { get; set; }
        public int Repetitions { get; set; }

        [ForeignKey(typeof(Routine))]
        public Guid RoutineId { get; set; }

        [ForeignKey(typeof(Exercise))]
        public Guid ExerciseId { get; set; }

        [ManyToOne]
        public Routine Routine { get; set; } = null!;

        [ManyToOne]
        public Exercise Exercise { get; set; } = null!;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Set> Sets { get; set; } = [];
    }
}
