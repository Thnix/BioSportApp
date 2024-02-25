using BioSportApp.Domain;
using BioSportApp.Models.Exercise;
using BioSportApp.Models.Set;
using BioSportApp.Models.Weight;
using Mapster;
using System.Collections.ObjectModel;

namespace BioSportApp
{
    public partial class App : Application
    {
        private readonly BioSportContext bioSportContext;

        public App(BioSportContext bioSportContext)
        {
            InitializeComponent();

            this.bioSportContext = bioSportContext;

            MainPage = new AppShell();

        }

        //protected override async void OnStart()
        //{
        //    await bioSportContext.CreateDbTables();
        //    App.ConfigureMappings();
        //}

        //private static void ConfigureMappings()
        //{
        //    TypeAdapterConfig<Exercise, ExerciseAddModel>.NewConfig()
        //        .Map(dest => dest.Sets, src => src.Sets.Adapt<ObservableCollection<SetAddModel>>());
        //}

    }
}
