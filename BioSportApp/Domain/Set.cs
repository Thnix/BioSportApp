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

        [Column("Number")]
        public string Number { get; set; } = string.Empty;

        [Column("Weight")]
        public decimal? Weight { get; set; }

        [ForeignKey(typeof(Exercise))]
        [Column("ExerciseId")]
        public Guid ExerciseId { get; set; }

        [ManyToOne]
        public Exercise? Exercise { get; set; }
    }
}
