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
using System.Windows.Media;
using System.Windows.Threading;

using MechanicTimer.Utilities;

namespace MechanicTimer.DataClasses
{

    internal class Mechanic : Notifier
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

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                NotifyPropertyChanged();
            }
        }

        private int currentTime;
        public int CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;

                TimerColour = ResourceCache.TIMER_DEFAULT;
                if (currentTime <= ResourceCache.AMBER_THRESHOLD)
                {
                    TimerColour = ResourceCache.TIMER_AMBER;
                }
                if (currentTime <= ResourceCache.RED_THRESHOLD)
                {
                    TimerColour = ResourceCache.TIMER_RED;
                }

                NotifyPropertyChanged();
            }
        }

        public Color timerColour;
        public Color TimerColour
        {
            get { return timerColour; }
            set
            {
                timerColour = value;
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

        public Step CurrentStep
        {
            get
            {
                return Steps != null && Steps.Count > 0 ? Steps[Index] : null;
            }
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

        private ICommand removeStepCommand;
        public ICommand RemoveStepCommand
        {
            get
            {
                if (removeStepCommand == null)
                {
                    removeStepCommand = new ButtonCommand(param => RemoveStep(), param => Steps.Count > 1);
                }
                return removeStepCommand;
            }
        }

        private DispatcherTimer Timer { get; } = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

        public Mechanic()
        {
            Name = ResourceCache.LOADING_TEXT;
            Start = 30;
            Frequency = 30;
            Delay = 5;
            Autostart = true;
            Visible = true;
            Steps = new ObservableCollection<Step>();

            Index = 0;
            CurrentTime = Start;
            TimerColour = ResourceCache.TIMER_DEFAULT;
            Timer.Tick += Timer_Tick;
        }

        public Mechanic(string name)
        {
            Name = name;
            Start = 30;
            Frequency = 30;
            Delay = 5;
            Autostart = true;
            Visible = true;
            Steps = new ObservableCollection<Step>() { new Step() };

            Index = 0;
            CurrentTime = Start;
            TimerColour = ResourceCache.TIMER_DEFAULT;
            Timer.Tick += Timer_Tick;
        }

        public Mechanic(string name, int start, int frequency, int delay, bool autostart, bool visible, List<Step> steps)
        {
            Name = name;
            Start = start;
            Frequency = frequency;
            Delay = delay;
            Autostart = autostart;
            Visible = visible;
            Steps = new ObservableCollection<Step>(steps);

            Index = 0;
            CurrentTime = Start;
            TimerColour = ResourceCache.TIMER_DEFAULT;
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
            Steps.Add(new Step());
        }

        public void RemoveStep()
        {
            Steps.RemoveAt(Steps.Count - 1);
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
    }
}
