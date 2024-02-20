using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging
{
    public class CloseStopWatchPopUp : ValueChangedMessage<bool>
    {
        public CloseStopWatchPopUp(bool value) : base(value)
        {

        }
    }
}
