using BioSportApp.ViewModels.Routine;

namespace BioSportApp.Pages.Routine;

public partial class RoutineListPage : ContentPage
{
    public RoutineListPage(RoutineListPageViewModel routineViewModel)
	{
		InitializeComponent();
        BindingContext = routineViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = (RoutineListPageViewModel)BindingContext;
        await viewModel.LoadAllRoutines();
    }
}