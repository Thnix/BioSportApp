using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain.Core
{
    public class Exercise
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ForeignKey(typeof(Category))]
        public Guid CategoryId { get; set; }

        [ManyToOne]
        public Category Category { get; set; } = null!;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<RoutineExercise> RoutineExercises { get; set; } = [];
    }
}
