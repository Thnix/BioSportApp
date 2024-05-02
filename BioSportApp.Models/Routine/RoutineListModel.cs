namespace BioSportApp.Models.Routine
{
    public class RoutineListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}
