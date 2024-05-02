using CommunityToolkit.Mvvm.ComponentModel;

namespace BioSportApp.ViewModels.Set
{
    public partial class SetViewModel : ObservableObject
    {
        [ObservableProperty]
        public Guid id = new();

        [ObservableProperty]
        public string setName = new("");

        [ObservableProperty]
        public decimal weight = new();

        [ObservableProperty]
        public Guid routineExerciseId = new();

        //[ObservableProperty]
        //public RoutineExercise routineExercise = new();
    }
}