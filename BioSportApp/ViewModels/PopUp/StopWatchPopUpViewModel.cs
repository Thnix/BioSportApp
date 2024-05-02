using BioSportApp.Messenger.CloseMessage;
using BioSportApp.Models.Lap;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BioSportApp.ViewModels.PopUp
{
    public partial class StopWatchPopUpViewModel : ObservableObject
    {
        private readonly Stopwatch stopwatch;
        private readonly IDispatcherTimer? timer;

        //Observable Properties

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


        //Methods

        /// <summary>
        /// Timer_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeElapsed = stopwatch.Elapsed.ToString(@"mm\:ss");
        }

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            stopwatch.Start();
            timer?.Start();
            IsRunning = true;
            BtnText = "Pausar";
        }

        /// <summary>
        /// Stop
        /// </summary>
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

        /// <summary>
        /// PLayOrPause
        /// </summary>
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

        /// <summary>
        /// Reset
        /// </summary>
        [RelayCommand]
        private void Reset()
        {
            LapNumber += 1;
            Laps.Add(new Lap { Name = LapNumber.ToString(), TimeElapsed = TimeElapsed });
            stopwatch.Reset();
            TimeElapsed = "00:00";
            IsRunning = false;
            BtnText = "Iniciar";
        }

        /// <summary>
        /// ClosePopUp
        /// </summary>
        [RelayCommand]
        public static void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseStopWatchPopUp(true));
        }
    }
}
