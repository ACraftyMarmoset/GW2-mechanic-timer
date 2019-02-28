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

namespace MechanicTimer.DataClasses
{
    internal class ResourceCache
    {
        private const string CONFIG_FILEPATH = "./config.json";

        public static ResourceCache Instance { get; } = new ResourceCache();

        public ObservableDictionary<string, Encounter> Encounters { get; set; }
        public ObservableDictionary<string, BitmapImage> Icons { get; set; }

        private ResourceCache()
        {
            LoadEncounters();
        }

        public BitmapImage GetIcon(string path)
        {
            if (Icons.ContainsKey(path))
            {
                return Icons[path];
            }
            else
            {
                Icons.Add(path, new BitmapImage(new Uri(path, UriKind.Relative)));
                return Icons[path];
            }
        }

        public void SaveEncounters()
        {
            File.WriteAllText(CONFIG_FILEPATH, JsonConvert.SerializeObject(Encounters));
        }

        private void LoadEncounters()
        {
            if (File.Exists(CONFIG_FILEPATH))
            {
                var data = JsonConvert.DeserializeObject<ObservableDictionary<string, Encounter>>(File.ReadAllText(CONFIG_FILEPATH));
                Encounters =  data ?? new ObservableDictionary<string, Encounter>();
            }
            else
            {
                File.Create(CONFIG_FILEPATH);
                Encounters = new ObservableDictionary<string, Encounter>();
            }
        }
    }
}
