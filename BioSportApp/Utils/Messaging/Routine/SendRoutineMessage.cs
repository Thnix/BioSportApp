using BioSportApp.Models.Routine.Messages;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging.Routine
{
    public class SendRoutineMessage : ValueChangedMessage<SendRoutineMessageModel>
    {
        public SendRoutineMessage(SendRoutineMessageModel value) : base(value)
        {

        }
    }
}
