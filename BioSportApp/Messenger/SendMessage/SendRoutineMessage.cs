using BioSportApp.Models.Routine;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.SendMessage
{
    public class SendRoutineMessage(RoutineAddModel routine) : ValueChangedMessage<RoutineAddModel>(routine)
    {
        public string Action { get; set; } = null!;
    }
}
