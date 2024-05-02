using BioSportApp.ViewModels.Routine;

namespace BioSportApp.Pages.Routine;

public partial class RoutineCreatePage : ContentPage
{
	public RoutineCreatePage(RoutineCreatePageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}