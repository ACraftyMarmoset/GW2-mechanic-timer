using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MechanicTimer.DataClasses
{
    internal class Encounter : INotifyPropertyChanged
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

        public Encounter() { }

        public Encounter(string name)
        {
            Name = name;
            Mechanics = new ObservableCollection<Mechanic>();
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
