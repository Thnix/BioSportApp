using BioSportApp.Models.RoutineExercise;

namespace BioSportApp.Models.Routine
{
    public class RoutineAddModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }

        public List<RoutineExerciseAddModel> RoutineExercises { get; set; } = [];
    }
}
