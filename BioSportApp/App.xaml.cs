
using BioSportApp.Services;
using Domain.Core;

namespace BioSportApp
{
    public partial class App : Application
    {
        private readonly BioSportContext context;
        private readonly CategoryService categoryService;
        private readonly ExerciseService exerciseService;
        public readonly IServiceProvider serviceProvider;

        public App(BioSportContext context, ExerciseService exerciseService, CategoryService categoryService, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            this.context = context;
            this.exerciseService = exerciseService;
            this.categoryService = categoryService;
            this.serviceProvider = serviceProvider;

            MainPage = new AutenticationShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            //Creating database tables
            await context.CreateDbTables();

            //Adding categories
            await categoryService.AddCategories();

            //Adding Exercises
            await exerciseService.AddShouldersExercises();
            await exerciseService.AddChestExercises();
            await exerciseService.AddAbdomenExercises();
            await exerciseService.AddBackExercises();
            await exerciseService.AddBicepsExercises();
            await exerciseService.AddTricepsExercises();
            await exerciseService.AddButtocksExercises();
            await exerciseService.AddLegsExercises();
            await exerciseService.AddCalvesExercises();
        }
    }
}
