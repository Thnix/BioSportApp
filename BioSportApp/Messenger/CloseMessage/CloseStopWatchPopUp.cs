using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.CloseMessage
{
    public class CloseStopWatchPopUp : ValueChangedMessage<bool>
    {
        public CloseStopWatchPopUp(bool value) : base(value)
        {

        }
    }
}
