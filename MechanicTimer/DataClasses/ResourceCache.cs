using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Newtonsoft.Json;

using MechanicTimer.Utilities;
using System.Windows.Input;
using System.Windows.Media;

namespace MechanicTimer.DataClasses
{
    internal class ResourceCache : Notifier
    {
        private const string CONFIG_FILEPATH = "./config.json";
        private const string IMAGE_FOLDER = "./Images/";

        public const string LOADING_TEXT = "Loading...";
        public const string DEFAULT_ICON = IMAGE_FOLDER + "zDefault.png";
        public const string DEFAULT_ENCOUNTER_NAME = "New Encounter";
        public const string DEFAULT_MECHANIC_NAME = "New Mechanic";
        public const string DEFAULT_STEP_NAME = "New Step";

        public const int AMBER_THRESHOLD = 10;
        public const int RED_THRESHOLD = 5;
        public static Color TIMER_DEFAULT = Colors.AntiqueWhite;
        public static Color TIMER_AMBER = Colors.Gold;
        public static Color TIMER_RED = Colors.OrangeRed;

        public static ResourceCache Instance { get; } = new ResourceCache();

        public ObservableCollection<Encounter> Encounters { get; private set; }
        public ObservableDictionary<string, BitmapImage> Icons { get; private set; }

        private Encounter currentEncounter;
        public Encounter CurrentEncounter
        {
            get { return currentEncounter; }
            set
            {
                currentEncounter = value;
                if (currentEncounter != null)
                {
                    foreach (var mechanic in currentEncounter.Mechanics)
                    {
                        mechanic.Reset();
                    }
                }
                NotifyPropertyChanged();
            }
        }

        private ICommand addEncounterCommand;
        public ICommand AddEncounterCommand
        {
            get
            {
                if (addEncounterCommand == null)
                {
                    addEncounterCommand = new ButtonCommand(param => AddEncounter(), param => true);
                }
                return addEncounterCommand;
            }
        }

        static ResourceCache()
        {

        }

        private ResourceCache()
        {
            LoadEncounters();
            LoadImages();

            CurrentEncounter = Encounters.Count > 0 ? Encounters[0] : null;
        }

        public static BitmapImage GetIcon(string path)
        {
            if (Instance.Icons.ContainsKey(path))
            {
                return Instance.Icons[path];
            }
            else
            {
                return Instance.Icons[DEFAULT_ICON];
            }
        }

        private void LoadImages()
        {
            Icons = new ObservableDictionary<string, BitmapImage>();

            foreach (var file in Directory.GetFiles(IMAGE_FOLDER, "*.png"))
            {
                Icons.Add(file, new BitmapImage(new Uri(file, UriKind.Relative)));
            }
        }

        public void AddEncounter()
        {
            Instance.Encounters.Add(new Encounter(DEFAULT_ENCOUNTER_NAME));
        }

        public void RemoveEncounter()
        {
            Instance.Encounters.Remove(Instance.CurrentEncounter);
            Instance.CurrentEncounter = Instance.Encounters.Count > 0 ? Instance.Encounters[0] : null;
        }

        public Encounter GetEncounter(string name)
        {
            try
            {
                return Instance.Encounters.First(x => x.Name == name);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }

        public void SaveEncounters()
        {
            File.WriteAllText(CONFIG_FILEPATH, JsonConvert.SerializeObject(Instance.Encounters));
        }

        private void LoadEncounters()
        {
            if (File.Exists(CONFIG_FILEPATH))
            {
                var data = JsonConvert.DeserializeObject<ObservableCollection<Encounter>>(File.ReadAllText(CONFIG_FILEPATH));
                Encounters = data ?? new ObservableCollection<Encounter>() { new Encounter() };
            }
            else
            {
                File.Create(CONFIG_FILEPATH);
                Encounters = new ObservableCollection<Encounter>() { new Encounter() };
            }
        }

        public void ReloadEncounters()
        {
            Instance.Encounters.Clear();
            if (File.Exists(CONFIG_FILEPATH))
            {
                var data = JsonConvert.DeserializeObject<ObservableCollection<Encounter>>(File.ReadAllText(CONFIG_FILEPATH));
                foreach (var encounter in data)
                {
                    Instance.Encounters.Add(encounter);
                }
            }

            if (Instance.Encounters.Count() > 0)
            {
                Instance.CurrentEncounter = Instance.Encounters[0];
            }
        }
    }
}
