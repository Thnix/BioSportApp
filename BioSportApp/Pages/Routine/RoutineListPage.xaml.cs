using BioSportApp.ViewModels.Routine;

namespace BioSportApp.Pages.Routine;

public partial class RoutineListPage : ContentPage
{
	public RoutineListPage(RoutineListPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}