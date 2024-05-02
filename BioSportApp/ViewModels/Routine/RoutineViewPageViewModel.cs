using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.PopUp;
using BioSportApp.Models.Routine;
using BioSportApp.Pages.Exercise;
using BioSportApp.Pages.Routine;
using BioSportApp.Services;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;

namespace BioSportApp.ViewModels.Routine
{
    [QueryProperty(nameof(RoutineId),"RoutineId")]
    public partial class RoutineViewPageViewModel : ObservableObject
    {
        private readonly RoutineService routineService;
        private readonly IPopupService popupService;

        public RoutineViewPageViewModel(RoutineService routineService, IPopupService popupService)
        {
            this.routineService = routineService;   
            this.popupService = popupService;

            WeakReferenceMessenger.Default.Register<SendRoutineMessage>(this, (r, m) =>
            {
                switch (m.Action)
                {
                    case "Update":
                        UpdateRoutine(m.Value);
                        break;

                    default:
                        break;
                }
            });
        }

        //Observable Properties
        [ObservableProperty]
        private string routineId = new("");

        [ObservableProperty]
        public RoutineViewModel routine = new();

        //Methods
        /// <summary>
        /// OnRoutineIdChanged
        /// </summary>
        /// <param name="value"></param>
        partial void OnRoutineIdChanged(string value)
        {
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
            }
            else
            {
                var messageData = new MessageDataModel
                {
                    IsValid = response.IsValid,
                    Message = response.Message
                };

                await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
            }
        }

        /// <summary>
        /// UpdateRoutine
        /// </summary>
        /// <param name="routineToUpdate"></param>
        private void UpdateRoutine(RoutineAddModel routineToUpdate)
        {
            Routine = routineToUpdate.Adapt<RoutineViewModel>();
        }

        /// <summary>
        /// ShowPopUpDeleteRoutine
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task ShowPopUpDeleteRoutine(string routineId)
        {
            await popupService.ShowPopupAsync<DeletePopUpViewModel>(onPresenting: vm => vm.RoutineToDelete = Routine.Adapt<RoutineAddModel>());
        }

        /// <summary>
        /// GoToEditPage
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task GoToEditPage(string routineId)
        {
            await Shell.Current.GoToAsync($"{nameof(RoutineCreatePage)}?RoutineId={routineId}");
        }

        /// <summary>
        /// GoToStartExercisePage
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task GoToStartExercisePage(Guid routineExerciseId)
        {
            await Shell.Current.GoToAsync($"{nameof(RoutineExerciseStartPage)}?RoutineExerciseId={routineExerciseId}");
        }
    }
}
