using BioSportApp.Common;
using BioSportApp.Models.Lap;
using BioSportApp.Utils.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BioSportApp.ViewModels.CustomPages
{
    public partial class StopWatchPopUpViewModel : ObservableObject
    {
        private readonly Stopwatch stopwatch;
        private readonly IDispatcherTimer? timer;

        [ObservableProperty]
        public bool isRunning = false;

        [ObservableProperty]
        public string btnText = "Iniciar";

        [ObservableProperty]
        private string timeElapsed;

        [ObservableProperty]
        public int lapNumber = 0;

        [ObservableProperty]
        private ObservableCollection<Lap> laps = [];

        public StopWatchPopUpViewModel()
        {
            stopwatch = new Stopwatch();
            TimeElapsed = "00:00";

            var dispatcher = Dispatcher.GetForCurrentThread();

            if(dispatcher != null)
            {
                timer = dispatcher.CreateTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.IsRepeating = true;

                timer.Tick += Timer_Tick;
            } 
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeElapsed = stopwatch.Elapsed.ToString(@"mm\:ss");
        }

        private void Start()
        {
            stopwatch.Start();
            timer?.Start();
            IsRunning = true;
            BtnText = "Pausar";
        }

        private void Stop()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                timer?.Stop();
                IsRunning = false;
                BtnText = "Continuar";
            }
        }

        [RelayCommand]
        private void PLayOrPause()
        {
            if(IsRunning)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }


        [RelayCommand]
        private void Reset()
        {
            LapNumber += 1;
            //Laps.Add(new Lap { Name = $"Vuelta {LapNumber}", TimeElapsed = TimeElapsed });
            Laps.Add(new Lap { Name = LapNumber.ToString(), TimeElapsed = TimeElapsed });
            stopwatch.Reset();
            TimeElapsed = "00:00";
            IsRunning = false;
            BtnText = "Iniciar";
        }

        [RelayCommand]
        public void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseStopWatchPopUp(true));

        }
    }
}
