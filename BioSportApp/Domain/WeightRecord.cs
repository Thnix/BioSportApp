using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain
{
    [Table("WeightRecord")]
    public class WeightRecord
    {
        [PrimaryKey]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Weight")]
        public decimal Weight { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }

        [ForeignKey(typeof(Exercise))]
        [Column("ExerciseId")]
        public Guid ExerciseId { get; set; }

        [ManyToOne]
        public Exercise? Exercise { get; set; }
    }
}
