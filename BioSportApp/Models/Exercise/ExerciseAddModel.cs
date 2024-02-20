using BioSportApp.Models.Set;
using BioSportApp.Models.Weight;
using System.Collections.ObjectModel;

namespace BioSportApp.Models.Exercise
{
    public class ExerciseAddModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int? SetsNumber { get; set; }

        public int? Repetitions { get; set; }

        public Guid RoutineId { get; set; }

        public ObservableCollection<SetAddModel> Sets { get; set; } = [];

        public ObservableCollection<WeightRecordAddModel> WeightRecords { get; set; } = [];
    }
}

