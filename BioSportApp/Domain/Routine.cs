using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace BioSportApp.Domain
{
    public class Routine
    {
        [PrimaryKey]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Date")]
        public DateTime Date { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        [Ignore]
        public ObservableCollection<Exercise> Exercises { get; set; } = [];
    }
}
