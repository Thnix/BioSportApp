using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain
{
    [Table("Set")]
    public class Set
    {
        [PrimaryKey]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("SetName")]
        public string SetName { get; set; } = string.Empty;

        [Column("CurrentNumber")]
        public int? CurrentNumber { get; set; }

        [Column("Weight")]
        public decimal? Weight { get; set; }

        [ForeignKey(typeof(Exercise))]
        [Column("ExerciseId")]
        public Guid ExerciseId { get; set; }

        [ManyToOne]
        public Exercise? Exercise { get; set; }
    }
}
