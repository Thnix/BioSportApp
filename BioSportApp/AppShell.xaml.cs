using BioSportApp.Pages.Exercise;
using BioSportApp.Pages.Routine;

namespace BioSportApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("RoutineCreatePage", typeof(RoutineCreatePage));
            Routing.RegisterRoute("RoutineViewPage", typeof(RoutineViewPage));
            Routing.RegisterRoute("ExerciseStartPage", typeof(ExerciseStartPage));
        }
    }
}
