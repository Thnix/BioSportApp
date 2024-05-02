using BioSportApp.ViewModels.RoutineExercise;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.Routine
{
    public partial class RoutineViewModel : ObservableObject
    {
        [ObservableProperty]
        public Guid id = new();

        [ObservableProperty]
        public string name = new("");

        [ObservableProperty]
        public DateTime creationDate = new();

        [ObservableProperty]
        public ObservableCollection<RoutineExerciseViewModel> routineExercises = [];
    }
}
