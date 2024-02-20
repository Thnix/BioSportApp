using BioSportApp.Common.CustomPages;
using BioSportApp.Domain;
using BioSportApp.Pages.Routine;
using BioSportApp.Services;
using BioSportApp.ViewModels.Routine;
using BioSportApp.ViewModels.CustomPages;
using BioSportApp.ViewModels.Exercise;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using BioSportApp.Pages.Exercise;
using BioSportApp.Pages.CustomPages;

namespace BioSportApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands-400.ttf", "FABrands");
                    fonts.AddFont("fa-solid-900.ttf", "FASolid");
                    fonts.AddFont("Parsi-Bold.ttf", "ParsiBold");
                });


            builder.Services.AddSingleton<BioSportContext>();

            //ViewModels
            builder.Services.AddTransient<RoutineCreatePageViewModel>();
            builder.Services.AddTransient<RoutineListPageViewModel>();
            builder.Services.AddTransient<ExerciseViewModel>();
            builder.Services.AddTransient<RoutineViewPageViewModel>();
            builder.Services.AddTransient<ExerciseStartPageViewModel>();


            //Pages
            builder.Services.AddTransient<RoutineCreatePage>();
            builder.Services.AddTransient<RoutineListPage>();
            builder.Services.AddTransient<RoutineViewPage>();
            builder.Services.AddTransient<ExerciseStartPage>();


            //Services
            builder.Services.AddTransient<RoutineService>();
            builder.Services.AddTransient<ExerciseService>();


            builder.Services.AddTransientPopup<DeletePopUp, DeletePopUpViewModel>();
            builder.Services.AddTransientPopup<SuperSetPopUp, SuperSetPopUpViewModel>();
            builder.Services.AddTransientPopup<StopWatchPopUp, StopWatchPopUpViewModel>();
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
