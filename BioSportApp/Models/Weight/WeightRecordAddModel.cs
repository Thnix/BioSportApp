
namespace BioSportApp.Models.Weight
{
    public class WeightRecordAddModel
    {
        public Guid Id { get; set; }

        public decimal Weight { get; set; }

        public DateTime Date { get; set; }

        public Guid ExerciseId { get; set; }
    }
}
