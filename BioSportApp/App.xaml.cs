using BioSportApp.Domain;

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

        protected override async void OnStart()
        {
            await bioSportContext.CreateDbTables();
        }
    }
}
