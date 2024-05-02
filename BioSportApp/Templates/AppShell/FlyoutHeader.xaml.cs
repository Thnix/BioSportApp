using BioSportApp.Common.Shared;
using BioSportApp.ViewModels.Template;

namespace BioSportApp.Templates.AppShell;

public partial class FlyoutHeader : ContentView
{
    public FlyoutHeader()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        var flyoutHeaderViewModel = SharedMethods.GetService<FlyoutHeaderViewModel>();

        if (flyoutHeaderViewModel != null)
        {
            BindingContext = flyoutHeaderViewModel;
        }
    }

}