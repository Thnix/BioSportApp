using CommunityToolkit.Mvvm.Messaging.Messages;
namespace BioSportApp.Messenger.CloseMessage
{
    public class CloseMessagePopUp : ValueChangedMessage<bool>
    {
        public CloseMessagePopUp(bool close) : base(close)
        {

        }
    }
}
