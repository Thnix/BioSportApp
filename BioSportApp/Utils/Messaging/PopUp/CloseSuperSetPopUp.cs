using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging.PopUp
{
    public class CloseSuperSetPopUp : ValueChangedMessage<bool>
    {
        public CloseSuperSetPopUp(bool value) : base(value)
        {
            
        }
    }
}
