using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json;

using MechanicTimer.DataClasses;
using System.Collections.ObjectModel;
using System.IO;

namespace MechanicTimer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ResourceCache.Instance;
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var choice = MessageBox.Show(this, "Do you want to save your changes?", "Save changes?", MessageBoxButton.YesNoCancel);
            if (choice != MessageBoxResult.Cancel)
            {
                if (choice == MessageBoxResult.Yes)
                {
                    ResourceCache.Instance.SaveEncounters();
                }
                Close();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceCache.Instance.CurrentEncounter != null)
            {
                foreach (var mechanic in ResourceCache.Instance.CurrentEncounter.Mechanics)
                {
                    mechanic.Reset();
                }
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            var choice = MessageBox.Show(this, "Are you sure you want to reload all encounters? You will lose any changes you've made.",
                                               "Reload Encounters?", MessageBoxButton.YesNo);
            if (choice == MessageBoxResult.Yes)
            {
                ResourceCache.Instance.ReloadEncounters();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var choice = MessageBox.Show(this, "Are you sure you want to delete this encounter? This cannot be undone.",
                                               "Delete Encounter?", MessageBoxButton.YesNo);
            if (choice == MessageBoxResult.Yes)
            {
                ResourceCache.Instance.RemoveEncounter();
            }
        }

        private void PlayAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceCache.Instance.CurrentEncounter != null)
            {
                foreach (var mechanic in ResourceCache.Instance.CurrentEncounter.Mechanics)
                {
                    if (mechanic.Autostart)
                    {
                        mechanic.Begin();
                    }
                }
            }
        }

        private void PauseAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceCache.Instance.CurrentEncounter != null)
            {
                foreach (var mechanic in ResourceCache.Instance.CurrentEncounter?.Mechanics)
                {
                    mechanic.Pause();
                }
            }
        }

        private void ResetAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceCache.Instance.CurrentEncounter != null)
            {
                foreach (var mechanic in ResourceCache.Instance.CurrentEncounter?.Mechanics)
                {
                        mechanic.Reset();
                }
            }
        }
    }
}
