using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.CloseMessage
{
    public class CloseSuperSetPopUp : ValueChangedMessage<bool>
    {
        public CloseSuperSetPopUp(bool close) : base(close)
        {

        }
    }
}
