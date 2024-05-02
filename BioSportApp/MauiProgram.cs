using BioSportApp.Pages.Exercise;
using BioSportApp.Pages.Home;
using BioSportApp.Pages.Login;
using BioSportApp.Pages.PopUp;
using BioSportApp.Pages.Routine;
using BioSportApp.Pages.User;
using BioSportApp.Services;
using BioSportApp.Services.Interfaces;
using BioSportApp.Templates.AppShell;
using BioSportApp.ViewModels.AppShell;
using BioSportApp.ViewModels.HomePage;
using BioSportApp.ViewModels.Login;
using BioSportApp.ViewModels.PopUp;
using BioSportApp.ViewModels.Routine;
using BioSportApp.ViewModels.RoutineExercise;
using BioSportApp.ViewModels.Template;
using BioSportApp.ViewModels.User;
using CommunityToolkit.Maui;
using Domain.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace BioSportApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit by adding the below line of code
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands-400.ttf", "FABrands");
                    fonts.AddFont("fa-solid-900.ttf", "FASolid");
                });


            //Creating local database
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "bioSportLocalContext");
            builder.Services.AddSingleton(new BioSportContext(dbPath));



            //Pages
            builder.Services.AddTransient<RoutineListPage>();
            builder.Services.AddTransient<RoutineCreatePage>();
            builder.Services.AddTransient<RoutineViewPage>();
            builder.Services.AddTransient<RoutineExerciseStartPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<UserCreatePage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<FlyoutHeader>();


            //ViewModels
            builder.Services.AddTransient<RoutineListPageViewModel>();
            builder.Services.AddTransient<RoutineCreatePageViewModel>();
            builder.Services.AddTransient<RoutineViewPageViewModel>();
            builder.Services.AddTransient<RoutineExerciseStartPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<UserCreatePageViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<FlyoutHeaderViewModel>();


            builder.Services.AddTransient<AppShellViewModel>();



            //Services

            //Transient
            builder.Services.AddTransient<RoutineService>();
            builder.Services.AddTransient<RoutineExerciseService>();
            builder.Services.AddTransient<SetService>();
            builder.Services.AddTransient<ExerciseService>();
            builder.Services.AddTransient<CategoryService>();
            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<LoginService>();

            //Singleton
            builder.Services.AddSingleton<PopupService>();
            builder.Services.AddSingleton<SessionService>();







            //PopUps
            builder.Services.AddTransientPopup<ExercisePickerPopUp, ExercisePickerPopUpViewModel>();
            builder.Services.AddTransientPopup<MessagePopUp, MessagePopUpViewModel>();
            builder.Services.AddTransientPopup<DeletePopUp, DeletePopUpViewModel>();
            builder.Services.AddTransientPopup<SuperSetPopUp, SuperSetPopUpViewModel>();
            builder.Services.AddTransientPopup<StopWatchPopUp, StopWatchPopUpViewModel>();


            //Interfaces
            builder.Services.AddSingleton<IDialogService, DialogService>();




#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
