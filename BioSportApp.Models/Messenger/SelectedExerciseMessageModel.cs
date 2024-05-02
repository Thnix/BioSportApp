using BioSportApp.Models.Exercise;

namespace BioSportApp.Models.Messenger
{
    public class SelectedExerciseMessageModel
    {
        public Guid TargetExerciseId { get; set; }
        public ExerciseListModel Exercise { get; set; } = new();
    }
}
