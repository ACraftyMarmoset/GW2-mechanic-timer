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

using MechanicTimer.DataClasses;

namespace MechanicTimer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ResourceCache.Instance;

            ResourceCache.Instance.Encounters.Add("Vale Guardian", new Encounter("Vale Guardian"));
            ResourceCache.Instance.Encounters.Add("Gorseval the Multifarious", new Encounter("Gorseval the Multifarious"));
            ResourceCache.Instance.Encounters.Add("Sabetha the Saboteur", new Encounter("Sabetha the Saboteur"));

            // Steps
            Step stpa = new Step("Step 1", "/Images/Marker_Arrow.png");
            Step stpb = new Step("Step 2", "/Images/Marker_Circle.png");
            Step stpc = new Step("Step 3", "/Images/Marker_Heart.png");
            Step stpd = new Step("Step 4", "/Images/Marker_Square.png");
            // Mechanics
            Mechanic mcha = new Mechanic("Mechanic 1", 20, 10, 5, true, false, new List<Step>() { stpa, stpb });
            Mechanic mchb = new Mechanic("Mechanic 2", 10, 20, 5, true, false, new List<Step>() { stpc, stpd });
            // Encounters
            ResourceCache.Instance.Encounters["Vale Guardian"].AddMechanic(mcha);
            ResourceCache.Instance.Encounters["Vale Guardian"].AddMechanic(mchb);
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
