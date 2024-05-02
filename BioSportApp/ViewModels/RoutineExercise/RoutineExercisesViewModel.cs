using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.Messenger;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.RoutineExercise
{
    public partial class RoutineExercisesViewModel : ObservableObject
    {
        public RoutineExercisesViewModel()
        {
            AddNewRoutineExercise();

            WeakReferenceMessenger.Default.Register<SendSelectedExerciseMessage>(this, (r, m) =>
            {
                SetExercise(m.Value);
            });
        }

        //Observable Properties

        [ObservableProperty]
        public ObservableCollection<RoutineExerciseViewModel> routineExercises = [];

        //Methods
        
        /// <summary>
        /// AddNewRoutineExercise
        /// </summary>
        [RelayCommand]
        public void AddNewRoutineExercise()
        {
            RoutineExercises.Add(new RoutineExerciseViewModel
            {
                Id = Guid.NewGuid(),
                Repetitions = "",
                SetsNumber = "",
                Name = "Seleccione un ejercicio",
            });
        }

        /// <summary>
        /// RemoveRoutineExercise
        /// </summary>
        /// <param name="routineExercise"></param>
        [RelayCommand]
        public void RemoveRoutineExercise(RoutineExerciseViewModel routineExercise)
        {
            RoutineExercises.Remove(routineExercise);
        }

        /// <summary>
        /// SetExercise
        /// </summary>
        /// <param name="data"></param>
        private void SetExercise(SelectedExerciseMessageModel data)
        {
            var exerciseToUpdate = RoutineExercises.SingleOrDefault(re => re.Id.ToString() == data.TargetExerciseId.ToString());

            if (exerciseToUpdate != null)
            {
                exerciseToUpdate.ExerciseId = data.Exercise.Id;
                exerciseToUpdate.Name = data.Exercise.Name;
            }
        }
    }
}
