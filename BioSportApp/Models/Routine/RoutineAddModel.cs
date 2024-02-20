

using BioSportApp.Models.Exercise;
using System.Collections.ObjectModel;

namespace BioSportApp.Models.Routine
{
    public class RoutineAddModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime Date { get; set; }

        public ObservableCollection<ExerciseAddModel> Exercises { get; set; } = [];
    }
}
