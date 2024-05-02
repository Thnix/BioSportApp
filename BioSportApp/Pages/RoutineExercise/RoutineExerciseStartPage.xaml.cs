using BioSportApp.ViewModels.RoutineExercise;

namespace BioSportApp.Pages.Exercise;

public partial class RoutineExerciseStartPage : ContentPage
{
	public RoutineExerciseStartPage(RoutineExerciseStartPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as IDisposable)?.Dispose();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
    }
}