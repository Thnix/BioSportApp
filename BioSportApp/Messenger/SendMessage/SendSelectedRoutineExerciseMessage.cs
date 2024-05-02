using BioSportApp.Models.RoutineExercise;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Messenger.SendMessage
{
    public class SendSelectedRoutineExerciseMessage : ValueChangedMessage<RoutineExerciseAddModel>
    {
        public SendSelectedRoutineExerciseMessage(RoutineExerciseAddModel routineExercise) : base(routineExercise)
        {

        }
    }
}
