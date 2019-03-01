using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MechanicTimer.Utilities;

namespace MechanicTimer.DataClasses
{
    internal class Encounter : INotifyPropertyChanged, IEquatable<Encounter>
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

        public ObservableCollection<Mechanic> Mechanics { get; }

        private ICommand addMechanicCommand;
        public ICommand AddMechanicCommand
        {
            get
            {
                if (addMechanicCommand == null)
                {
                    addMechanicCommand = new ButtonCommand(param => AddMechanic(new Mechanic()), param => true);
                }
                return addMechanicCommand;
            }
        }

        private ICommand removeMechanicCommand;
        public ICommand RemoveMechanicCommand
        {
            get
            {
                if (removeMechanicCommand == null)
                {
                    removeMechanicCommand = new ButtonCommand(param => RemoveMechanic(), param => Mechanics.Count > 1);
                }
                return removeMechanicCommand;
            }
        }

        public Encounter()
        {
            Name = "New Encounter";
            Mechanics = new ObservableCollection<Mechanic>() { new Mechanic() };
        }

        public Encounter(string name)
        {
            Name = name;
            Mechanics = new ObservableCollection<Mechanic>() { new Mechanic() };
        }

        public Encounter(string name, List<Mechanic> mechanics)
        {
            Name = name;
            Mechanics = new ObservableCollection<Mechanic>(mechanics);
        }

        public void AddMechanic(Mechanic mechanic)
        {
            Mechanics.Add(mechanic);
        }

        public void RemoveMechanic()
        {
            Mechanics.RemoveAt(Mechanics.Count - 1);
        }

        public bool Equals(Encounter other)
        {
            return Name == other.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
