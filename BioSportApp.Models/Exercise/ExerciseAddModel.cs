using BioSportApp.Models.RoutineExercise;

namespace BioSportApp.Models.Exercise
{
    public class ExerciseAddModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public List<RoutineExerciseAddModel> RoutineExercises { get; set; } = [];
    }
}
