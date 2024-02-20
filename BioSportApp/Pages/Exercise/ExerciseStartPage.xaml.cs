using BioSportApp.ViewModels.Exercise;

namespace BioSportApp.Pages.Exercise;

public partial class ExerciseStartPage : ContentPage
{
	public ExerciseStartPage(ExerciseStartPageViewModel viewModel)
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