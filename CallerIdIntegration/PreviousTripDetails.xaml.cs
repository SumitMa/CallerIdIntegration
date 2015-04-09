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
using System.Windows.Shapes;

namespace CallerIdIntegration
{
    /// <summary>
    /// Interaction logic for PreviousTripDetails.xaml
    /// </summary>
    public partial class PreviousTripDetails : Window
    {
        public PreviousTripDetails()
        {
            InitializeComponent();
            this.Loaded += PreviousTripDetails_Loaded;
        }

        void PreviousTripDetails_Loaded(object sender, RoutedEventArgs e)
        {
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            List<Da> objda = new List<Da>();
            Da obj = new Da();
            obj.TelephoneNumber = "6546464646";
            obj.PickUpAddress = "sdfafdasdf";
            obj.DropOffAddress = "asdfasfasdf";
            objda.Add(obj);
            Da obj1 = new Da();
            obj1.TelephoneNumber = "6546464646";
            obj1.PickUpAddress = "adfafdasdf";
            obj1.DropOffAddress = "bsdfasfasdf";
            objda.Add(obj1);
            DataGridPreviousDetails.ItemsSource = objda;

        }

        public class Da
        {
            public string TelephoneNumber { get; set; }
            public string PickUpAddress { get; set; }
            public string DropOffAddress { get; set; }
        }
    }
}
