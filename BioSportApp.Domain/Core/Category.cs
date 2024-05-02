using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BioSportApp.Domain.Core
{
    public class Category
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Exercise> Exercises { get; set; } = [];
    }
}
