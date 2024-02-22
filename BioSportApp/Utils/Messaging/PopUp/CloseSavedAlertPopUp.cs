using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging.PopUp
{
    public class CloseSavedAlertPopUp : ValueChangedMessage<bool>
    {
        public CloseSavedAlertPopUp(bool value) : base(value)
        {
            
        }
    }
}
