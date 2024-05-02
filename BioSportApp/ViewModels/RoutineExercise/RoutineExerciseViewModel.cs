using BioSportApp.ViewModels.Set;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.RoutineExercise
{
    public partial class RoutineExerciseViewModel : ObservableObject
    {
        [ObservableProperty]
        public Guid id = new();

        [ObservableProperty]
        public string name = new("");

        [ObservableProperty]
        public string setsNumber = new("");

        [ObservableProperty]
        public string repetitions = new("");

        [ObservableProperty]
        public Guid routineId = new();

        [ObservableProperty]
        public Guid exerciseId = new();

        [ObservableProperty]
        public ObservableCollection<SetViewModel> sets = [];
    }
}
