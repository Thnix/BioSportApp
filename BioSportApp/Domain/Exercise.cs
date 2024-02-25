using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace BioSportApp.Domain
{
    [Table("Exercise")]
    public class Exercise
    {
        [PrimaryKey]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; } = null!;

        [Column("SetsNumber")]
        public int? SetsNumber { get; set; }

        [Column("Repetitions")]
        public int? Repetitions { get; set; }

        [ForeignKey(typeof(Routine))]
        [Column("RoutineId")]
        public Guid RoutineId { get; set; }

        [ManyToOne]
        public Routine? Routine { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        
        public ObservableCollection<Set> Sets { get; set; } = [];

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        //[Ignore]
        //public ObservableCollection<WeightRecord> WeightRecords { get; set; } = [];
    }
}