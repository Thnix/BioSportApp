using BioSportApp.Common.Shared;
using BioSportApp.Pages.Exercise;
using BioSportApp.Pages.Routine;
using BioSportApp.Pages.User;
using BioSportApp.ViewModels.AppShell;

namespace BioSportApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        /// <summary>
        /// OnLoaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object? sender, EventArgs e)
        {
            var appShellVm = SharedMethods.GetService<AppShellViewModel>();

            if (appShellVm != null)
            {
                BindingContext = appShellVm;
            }

            RegisterRoutes();
        }

        /// <summary>
        /// RegisterRoutes
        /// </summary>
        private void RegisterRoutes()
        {
            Routing.RegisterRoute("RoutineCreatePage", typeof(RoutineCreatePage));
            Routing.RegisterRoute("RoutineViewPage", typeof(RoutineViewPage));
            Routing.RegisterRoute("RoutineExerciseStartPage", typeof(RoutineExerciseStartPage));
            Routing.RegisterRoute("RoutineListPage", typeof(RoutineListPage));
            Routing.RegisterRoute("UserCreatePage", typeof(UserCreatePage));
        }
    }
}
