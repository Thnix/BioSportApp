using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.CloseMessage
{
    public class CloseDeleteRoutinePopUp : ValueChangedMessage<bool>
    {
        public CloseDeleteRoutinePopUp(bool close) : base(close)
        {

        }
    }
}
