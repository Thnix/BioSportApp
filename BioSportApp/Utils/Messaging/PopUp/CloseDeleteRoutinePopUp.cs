using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging.Routine
{
    public class CloseDeleteRoutinePopUp : ValueChangedMessage<bool>
    {
        public CloseDeleteRoutinePopUp(bool value) : base(value)
        {
            
        }
    }
}
