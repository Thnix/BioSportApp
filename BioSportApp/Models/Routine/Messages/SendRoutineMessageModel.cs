
namespace BioSportApp.Models.Routine.Messages
{
    public class SendRoutineMessageModel
    {
        public RoutineAddModel Routine { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
