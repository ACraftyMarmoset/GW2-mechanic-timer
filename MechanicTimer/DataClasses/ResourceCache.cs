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

namespace MechanicTimer.DataClasses
{
    internal class ResourceCache
    {
        private const string CONFIG_FILEPATH = "./config.json";

        private static readonly ResourceCache instance = new ResourceCache();
        public static ResourceCache Instance
        {
            get
            {
                return instance;
            }
        }

        public ObservableCollection<Encounter> Encounters { get; set; }
        public ObservableDictionary<string, BitmapImage> Icons { get; set; }

        public Encounter CurrentEncounter { get; set; }

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

        private ICommand removeEncounterCommand;
        public ICommand RemoveEncounterCommand
        {
            get
            {
                if (removeEncounterCommand == null)
                {
                    removeEncounterCommand = new ButtonCommand(param => RemoveEncounter(), param => true);
                }
                return removeEncounterCommand;
            }
        }

        static ResourceCache()
        {

        }

        private ResourceCache()
        {
            Console.WriteLine("Constructor running");
            LoadEncounters();
            LoadImages();
        }

        public static BitmapImage GetIcon(string path)
        {
            if (Instance.Icons.ContainsKey(path))
            {
                return Instance.Icons[path];
            }
            else
            {
                Instance.Icons.Add(path, new BitmapImage(new Uri(path, UriKind.Relative)));
                return Instance.Icons[path];
            }
        }

        private void LoadImages()
        {
            Icons = new ObservableDictionary<string, BitmapImage>();

            foreach (var file in Directory.GetFiles("./Images/", "*.png"))
            {
                Icons.Add(file, new BitmapImage(new Uri(file, UriKind.Relative)));
            }
        }

        public void AddEncounter()
        {
            Instance.Encounters.Add(new Encounter($"New Encounter"));
        }

        public void RemoveEncounter()
        {
            Instance.Encounters.Remove(Instance.CurrentEncounter);
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
                Console.WriteLine("Found File");
                var data = JsonConvert.DeserializeObject<ObservableCollection<Encounter>>(File.ReadAllText(CONFIG_FILEPATH));
                Encounters =  data ?? new ObservableCollection<Encounter>();
            }
            else
            {
                File.Create(CONFIG_FILEPATH);
                Encounters = new ObservableCollection<Encounter>();
            }
        }
    }
}
