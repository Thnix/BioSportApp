using BioSportApp.Models.Exercise;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging.Exercise
{
    public class SendExerciseMessage : ValueChangedMessage<ExerciseAddModel>
    {
        public SendExerciseMessage(ExerciseAddModel value) : base(value)
        {

        }
    }
}
