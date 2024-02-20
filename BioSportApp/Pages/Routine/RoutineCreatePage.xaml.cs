using BioSportApp.ViewModels.Routine;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.Routine;

public partial class RoutineCreatePage : ContentPage
{
	public RoutineCreatePage(RoutineCreatePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}