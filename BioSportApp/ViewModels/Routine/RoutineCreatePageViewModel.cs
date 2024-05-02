using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.PopUp;
using BioSportApp.Models.Routine;
using BioSportApp.Services;
using BioSportApp.ViewModels.PopUp;
using BioSportApp.ViewModels.RoutineExercise;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.Routine
{
    [QueryProperty(nameof(RoutineId), "RoutineId")]
    public partial class RoutineCreatePageViewModel : ObservableObject
    {
        public RoutineExercisesViewModel RoutineExercisesViewModel { get; } = new RoutineExercisesViewModel();

        private readonly IPopupService popupService;
        private readonly RoutineService routineService;

        public RoutineCreatePageViewModel(IPopupService popupService, RoutineService routineService)
        {
            this.popupService = popupService;
            this.routineService = routineService;
        }

        //Observable Properties
        [ObservableProperty]
        public RoutineViewModel routine = new();

        [ObservableProperty]
        private string routineId = new("");

        [ObservableProperty]
        private string pageTitle = new("Crear Rutina");

        //Methods

        //Methods
        /// <summary>
        /// OnRoutineIdChanged
        /// </summary>
        /// <param name="value"></param>
        partial void OnRoutineIdChanged(string value)
        {
            PageTitle = "Editar Rutina";
            LoadRoutine(value);
        }

        /// <summary>
        /// LoadRoutine
        /// </summary>
        /// <param name="routineId"></param>
        private async void LoadRoutine(string routineId)
        {
            var response = await routineService.GetRoutineViewById(Guid.Parse(routineId));

            if (response.IsValid && response != null)
            {
                Routine = response.Data.Adapt<RoutineViewModel>();
                RoutineExercisesViewModel.RoutineExercises = response.Data.RoutineExercises.Adapt<ObservableCollection<RoutineExerciseViewModel>>();
            }
            else
            {
                var messageData = new MessageDataModel
                {
                    IsValid = response.IsValid,
                    Message = response.Message
                };

                await ShowMessage(messageData);
            }
        }


        /// <summary>
        /// CreateOrUpdateRoutine
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task CreateOrUpdateRoutine(RoutineViewModel routine)
        {
            routine.RoutineExercises = RoutineExercisesViewModel.RoutineExercises;

            var messageData = ValidateFields(routine);

            if (!messageData.IsValid)
            {
                await ShowMessage(messageData);
                return;
            }

            if (string.IsNullOrEmpty(RoutineId))
            {
                await CreateRoutine(routine, messageData);
            }
            else
            {
                await UpdateRoutine(routine, messageData);
            }
        }

        /// <summary>
        /// CreateRoutine
        /// </summary>
        /// <returns></returns>
        private async Task CreateRoutine(RoutineViewModel routineToAdd, MessageDataModel messageData)
        {
            var response = await routineService.CreateRoutine(routineToAdd.Adapt<RoutineAddModel>());

            if (response.IsValid && response.Data != null)
            {
                var routineToSend = routineToAdd.Adapt<RoutineAddModel>();
                routineToSend.Id = response.Data.Id;
                routineToSend.CreationDate = DateTime.Now;

                WeakReferenceMessenger.Default
                    .Send(new SendRoutineMessage(routineToSend) { Action = "Create" });
            };

            messageData.IsValid = response.IsValid;
            messageData.Message = response.Message;

            await ShowMessage(messageData);
            await Shell.Current.Navigation.PopAsync();
        }

        /// <summary>
        /// UpdateRoutine
        /// </summary>
        /// <param name="routineToUpdate"></param>
        /// <returns></returns>
        private async Task UpdateRoutine(RoutineViewModel routineToUpdate, MessageDataModel messageData)
        {
            var response = await routineService.UpdateRoutine(routineToUpdate.Adapt<RoutineAddModel>());

            if (response.IsValid)
            {
                var routineToSend = routineToUpdate.Adapt<RoutineAddModel>();

                WeakReferenceMessenger.Default
                    .Send(new SendRoutineMessage(routineToSend) { Action = "Update" });
            };

            messageData.IsValid = response.IsValid;
            messageData.Message = response.Message;

            await ShowMessage(messageData);
            await Shell.Current.Navigation.PopAsync();
        }

        /// <summary>
        /// ShowMessage
        /// </summary>
        /// <param name="messageData"></param>
        /// <returns></returns>
        private async Task ShowMessage(MessageDataModel messageData)
        {
            await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
        }

        /// <summary>
        /// ValidateFields
        /// </summary>
        /// <param name="routineToAdd"></param>
        /// <returns></returns>
        private MessageDataModel ValidateFields(RoutineViewModel routineToAdd)
        {
            var messageData = new MessageDataModel();

            bool isRoutineNameInvalid = string.IsNullOrEmpty(routineToAdd.Name);

            bool areExercisesInvalid = routineToAdd.RoutineExercises
                .Any(routineExercise =>
                    string.IsNullOrEmpty(routineExercise.Name) || routineExercise.Name == "Seleccione un ejercicio" ||
                    string.IsNullOrEmpty(routineExercise.SetsNumber) ||
                    string.IsNullOrEmpty(routineExercise.Repetitions));

            bool existExercises = routineToAdd.RoutineExercises.Any();

            if (isRoutineNameInvalid)
            {
                messageData.Message += "El nombre de la rutina es necesario.\n\n";
            }

            if (!existExercises)
            {
                messageData.Message += "Agregue al menos un ejercicio.\n\n";
            }

            if (areExercisesInvalid)
            {
                messageData.Message += "Complete los campos de los ejercicios.";
            }

            messageData.IsValid = !isRoutineNameInvalid && !areExercisesInvalid && existExercises;

            return messageData;
        }

        //Navigation

        //PopUps

        /// <summary>
        /// OpenExercisePickerPopUp
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task OpenExercisePickerPopUp(Guid Id)
        {
            await popupService.ShowPopupAsync<ExercisePickerPopUpViewModel>(vm => vm.TargetExerciseId = Id);
        }
    }
}
