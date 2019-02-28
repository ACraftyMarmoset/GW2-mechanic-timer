using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MechanicTimer.DataClasses
{

    internal class Mechanic : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Step> Steps { get; }

        private int start;
        public int Start
        {
            get { return start; }
            set
            {
                start = value;
                NotifyPropertyChanged();
            }
        }

        private int frequency;
        public int Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                NotifyPropertyChanged();
            }
        }

        private int delay;
        public int Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                NotifyPropertyChanged();
            }
        }

        private bool autostart;
        public bool Autostart
        {
            get { return autostart; }
            set
            {
                autostart = value;
                NotifyPropertyChanged();
            }
        }

        private bool autohide;
        public bool Autohide
        {
            get { return autohide; }
            set
            {
                autohide = value;
                NotifyPropertyChanged();
            }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CurrentStep");
            }
        }

        private int currentTime;
        public int CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                NotifyPropertyChanged();
            }
        }

        public Step CurrentStep
        {
            get { return Steps[Index]; }
        }

        private ICommand beginCommand;
        public ICommand BeginCommand
        {
            get
            {
                if (beginCommand == null)
                {
                    beginCommand = new ButtonCommand(param => Begin(), param => !Timer.IsEnabled);
                }
                return beginCommand;
            }
        }

        private ICommand pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                {
                    pauseCommand = new ButtonCommand(param => Pause(), param => Timer.IsEnabled);
                }
                return pauseCommand;
            }
        }

        private ICommand resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new ButtonCommand(param => Reset(), param => true);
                }
                return resetCommand;
            }
        }

        private ICommand addStepCommand;
        public ICommand AddStepCommand
        {
            get
            {
                if (addStepCommand == null)
                {
                    addStepCommand = new ButtonCommand(param => AddStep(), param => true);
                }
                return addStepCommand;
            }
        }

        private DispatcherTimer Timer { get; } = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

        public Mechanic() { }

        public Mechanic(string name, int start, int frequency, int delay, bool autostart, bool autohide, List<Step> steps)
        {
            Name = name;
            Start = start;
            Frequency = frequency;
            Delay = delay;
            Autostart = autostart;
            Autohide = autohide;
            Steps = new ObservableCollection<Step>(steps);

            Index = 0;
            CurrentTime = Start;
            Timer.Tick += Timer_Tick;
        }

        public void Begin()
        {
            Timer.Start();
        }

        public void Pause()
        {
            Timer.Stop();
        }

        public void Reset()
        {
            Timer.Stop();
            CurrentTime = Start;
            Index = 0;
        }

        public void AddStep()
        {
            Steps.Add(new Step("New Step", "/Images/Default.png"));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime -= 1;
            if (CurrentTime < -Delay)
            {
                Index = (Index + 1) % Steps.Count();
                CurrentTime = Frequency - Delay;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class ButtonCommand : ICommand
        {
            private readonly Action<object> execute;
            public void Execute(object param)
            {
                execute(param);
            }

            private readonly Predicate<object> canExecute;
            public bool CanExecute(object param)
            {
                if (canExecute != null)
                {
                    return canExecute(param);
                }
                return true;
            }
            
            public ButtonCommand(Action<object> action, Predicate<object> predicate)
            {
                execute = action;
                canExecute = predicate;
            }

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    CommandManager.RequerySuggested += value;
                }
                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
    }
}
