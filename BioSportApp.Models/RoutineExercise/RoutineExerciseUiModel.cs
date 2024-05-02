namespace BioSportApp.Models.RoutineExercise
{
    public class RoutineExerciseUiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SetsNumber { get; set; }
        public int Repetitions { get; set; }
        public Guid RoutineId { get; set; }
        public Guid ExerciseId { get; set; }
    }
}

