using CommunityToolkit.Mvvm.ComponentModel;

namespace BioSportApp.ViewModels.Exercise
{
    public partial class ExerciseListViewModel: ObservableObject
    {
        [ObservableProperty]
        public Guid id = new();

        [ObservableProperty]
        public string name = new("");
    }
}
