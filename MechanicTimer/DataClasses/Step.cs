using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MechanicTimer.DataClasses
{
    internal class Step : INotifyPropertyChanged
    {
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        private BitmapImage icon;
        public BitmapImage Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                NotifyPropertyChanged();
            }
        }

        public Step()
        {
            Description = "New Step";
            Icon = ResourceCache.GetIcon("./Images/Default.png");
        }

        public Step(string description, BitmapImage icon)
        {
            Description = description;
            Icon = icon;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
