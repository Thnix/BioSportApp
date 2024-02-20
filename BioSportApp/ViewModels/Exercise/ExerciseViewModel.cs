
using BioSportApp.Models.Exercise;
using BioSportApp.Models.Routine;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.Exercise
{
    public partial class ExerciseViewModel : ObservableObject
    {
        public ExerciseViewModel()
        {

        }

        [ObservableProperty]
        public ObservableCollection<ExerciseAddModel> exercises = [];

        [ObservableProperty]
        private ExerciseAddModel exercise = new();

        [RelayCommand]
        public void NewExercise()
        {
            var newExercise = new ExerciseAddModel
            {
                Id = Guid.NewGuid(),
                Name = "",
                SetsNumber = null,
                Repetitions = null
            };

            Exercises.Add(newExercise);
        }

        [RelayCommand]
        private void RemoveExercise(ExerciseAddModel exercise)
        {
            Exercises.Remove(exercise);
        }
    }
}
