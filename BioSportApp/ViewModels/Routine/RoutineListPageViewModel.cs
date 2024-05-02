using BioSportApp.Domain.Core;
using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.PopUp;
using BioSportApp.Models.Routine;
using BioSportApp.Pages.Routine;
using BioSportApp.Services;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.Routine
{
    public partial class RoutineListPageViewModel : ObservableObject
    {
        private readonly RoutineService routineService;
        private readonly IPopupService popupService;

        public RoutineListPageViewModel(RoutineService routineService, IPopupService popupService)
        {
            this.routineService = routineService;
            this.popupService = popupService;

            Initialize();

            WeakReferenceMessenger.Default.Register<SendRoutineMessage>(this, (r,m) =>
            {
                switch (m.Action)
                {
                    case "Create": 
                        AddRoutineToList(m.Value);
                        break;

                    case "Update":
                        UpdateRoutineInList(m.Value);
                        break;

                    case "Delete":
                        DeleteRoutineInList(m.Value);
                        break;

                    default: 
                        break;
                }
            });
        }

        [ObservableProperty]
        public ObservableCollection<RoutineListViewModel> routines = [];

        [ObservableProperty]
        public RoutineListViewModel? selectedRoutine = new();

        //Methods

        /// <summary>
        /// Initialize
        /// </summary>
        private async void Initialize()
        {
            await GetAllRoutines();
        }

        /// <summary>
        /// AddRoutineToList
        /// </summary>
        /// <param name="routineToAdd"></param>
        private void AddRoutineToList(RoutineAddModel routineToAdd)
        {
            Routines.Add(routineToAdd.Adapt<RoutineListViewModel>());
        }

        /// <summary>
        /// UpdateRoutineInList
        /// </summary>
        /// <param name="routineToUpdate"></param>
        private void UpdateRoutineInList(RoutineAddModel routineToUpdate)
        {
            var routine = Routines.SingleOrDefault(rt => rt.Id == routineToUpdate.Id);

            if (routine != null)
            {
                var index = Routines.IndexOf(routine);
                Routines[index] = routineToUpdate.Adapt<RoutineListViewModel>();
            }
        }

        /// <summary>
        /// DeleteRoutineInList
        /// </summary>
        /// <param name="routineToUpdate"></param>
        private void DeleteRoutineInList(RoutineAddModel routineToDelete)
        {
            var routine = Routines.SingleOrDefault(x => x.Id == routineToDelete.Id);

            if (routine != null)
            {
                Routines.Remove(routine);
            }
        }

        /// <summary>
        /// GetAllRoutines
        /// </summary>
        /// <returns></returns>
        private async Task GetAllRoutines()
        {
            var response = await routineService.GetAllRoutines();

            if (response.IsValid && response.Data != null) 
            {
                Routines = response.Data.Adapt<ObservableCollection<RoutineListViewModel>>();
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


        //Navigation

        /// <summary>
        /// GoToCreateRoutinePage
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task GoToCreateRoutinePage()
        {
            await Shell.Current.GoToAsync(nameof(RoutineCreatePage));
        }

        /// <summary>
        /// OnSelectedRoutineChanged
        /// </summary>
        /// <param name="value"></param>
        async partial void OnSelectedRoutineChanged(RoutineListViewModel? value)
        {
            if(value != null)
            {
                await Shell.Current.GoToAsync($"{nameof(RoutineViewPage)}?RoutineId={value.Id}");
                SelectedRoutine = null;
            }
        }
    }
}
