namespace BioSportApp.Models.Set
{
    public class SetAddModel
    {
        public Guid Id { get; set; }
        public string SetName { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public Guid RoutineExerciseId { get; set; }
    }
}
