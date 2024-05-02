using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.CloseMessage
{
    public class CloseExercisePickerPopUp : ValueChangedMessage<bool>
    {
        public CloseExercisePickerPopUp(bool close) : base(close)
        {

        }
    }
}
