using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BioSportApp.Utils.Messaging.Routine
{
    public class IsRoutineLoading : ValueChangedMessage<bool>
    {
        public IsRoutineLoading(bool value) : base(value)
        {

        }
    }
}
