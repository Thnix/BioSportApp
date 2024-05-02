using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain.Core
{
    public class Set
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string SetName { get; set; } = string.Empty;
        public int CurrentNumber { get; set; }
        public decimal Weight { get; set; }

        [ForeignKey(typeof(RoutineExercise))]
        public Guid RoutineExerciseId { get; set; }

        [ManyToOne]
        public RoutineExercise RoutineExercise { get; set; } = null!;
    }
}