using BioSportApp.Models.Exercise;

namespace BioSportApp.Models.Set
{
    public class SetAddModel
    {
        public Guid Id { get; set; }

        public string SetName { get; set; } = string.Empty;

        public int? CurrentNumber { get; set; }

        public decimal? Weight { get; set; }

        public Guid ExerciseId { get; set; }

        //public ExerciseAddModel? Exercise { get; set; }
    }
}