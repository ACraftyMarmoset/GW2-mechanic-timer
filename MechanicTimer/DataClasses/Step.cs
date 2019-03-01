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

        private string icon;
        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                NotifyPropertyChanged();
            }
        }

        public BitmapImage IconSource
        {
            get
            {
                return ResourceCache.GetIcon(Icon);
            }
        }

        public Step()
        {
            Description = ResourceCache.DEFAULT_STEP_NAME;
            Icon = ResourceCache.DEFAULT_ICON;
        }

        public Step(string description, string icon)
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
