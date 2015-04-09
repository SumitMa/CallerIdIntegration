using CallerIdIntegration.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CallerIdIntegration.UserControl
{
    /// <summary>
    /// Interaction logic for AddContactsUserControl.xaml
    /// </summary>
    public partial class AddContactsUserControl
    {
        private Popup PopupCallerId;
        private  PCApp_TaxitechEntities _objEntities = null;

        #region Instance Cunstructor
        public AddContactsUserControl()
        {
            InitializeComponent();
        }

        public AddContactsUserControl(Popup PopupCallerId)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.PopupCallerId = PopupCallerId;
        }
        #endregion

        #region Save Contact
        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBoxCustomerName.Text = string.Empty;
                TextBoxPhoneNumber.Text = string.Empty;
                Style style = this.FindResource("BCTextBoxBorder") as Style;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#CACACA");

                BorderPhoneNumber.BorderBrush = brush;
                BorderCustomerName.BorderBrush = brush;
                TextBlockMessage.Text = string.Empty;
                TextBoxCustomerName.Focus();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#CACACA");

                string customerName = TextBoxCustomerName.Text.Trim();
                string PhoneNumber = TextBoxPhoneNumber.Text.Trim();
                if (customerName == string.Empty)
                {
                    BorderCustomerName.BorderBrush = Brushes.Red;
                    return;
                }
                else
                {
                    BorderCustomerName.BorderBrush = brush;
                }
                if (PhoneNumber == string.Empty)
                {
                    BorderPhoneNumber.BorderBrush = Brushes.Red;
                    return;
                }
                else
                {
                    BorderPhoneNumber.BorderBrush = brush;
                }

                _objEntities = new PCApp_TaxitechEntities();
                contact objModel = new contact();
                objModel.Name = customerName;
                objModel.Phone = PhoneNumber;
                _objEntities.contacts.Add(objModel);
                int Result = _objEntities.SaveChanges();
                if (Result == 1)
                    TextBlockMessage.Text = "Save Successfully";
                else
                    TextBlockMessage.Text = "Error in saving data.";

                TextBoxCustomerName.Text = string.Empty;
                TextBoxPhoneNumber.Text = string.Empty;
                TextBoxCustomerName.Focus();
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion
    }
}
