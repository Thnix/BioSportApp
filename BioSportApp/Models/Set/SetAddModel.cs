using BioSportApp.Models.Exercise;

namespace BioSportApp.Models.Set
{
    public class SetAddModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public decimal? Weight { get; set; }

        public Guid ExerciseId { get; set; }

        //public ExerciseAddModel? Exercise { get; set; }
    }
}