using CommunityToolkit.Mvvm.ComponentModel;

namespace BioSportApp.ViewModels.Routine
{
    public partial class RoutineListViewModel : ObservableObject
    {
        [ObservableProperty]
        public Guid id = new();

        [ObservableProperty]
        public string name = new("");

        [ObservableProperty]
        public DateTime creationDate = new();
    }
}
