using BioSportApp.ViewModels.Routine;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.Routine;

public partial class RoutineViewPage : ContentPage
{
	public RoutineViewPage(RoutineViewPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}