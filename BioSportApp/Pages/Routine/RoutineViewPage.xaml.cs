using BioSportApp.ViewModels.Routine;

namespace BioSportApp.Pages.Routine;

public partial class RoutineViewPage : ContentPage
{
	public RoutineViewPage(RoutineViewPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}