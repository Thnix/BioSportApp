using BioSportApp.Models.Messenger;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.SendMessage
{
    public class SendSelectedExerciseMessage : ValueChangedMessage<SelectedExerciseMessageModel>
    {
        public SendSelectedExerciseMessage(SelectedExerciseMessageModel selectedExercise) : base(selectedExercise)
        {

        }
    }
}
