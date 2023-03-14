using WPF_Front.Common;

namespace WPF_Front.Views.Tabs
{
    /// <summary>
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            PrinterNameField.Text = XmlLogic.SyncXmlValue(XmlLogic.PrinterNameXmlNode);
            DbNameField.Text = XmlLogic.SyncXmlValue(XmlLogic.DbNameXmlNode);
            DbServerField.Text = XmlLogic.SyncXmlValue(XmlLogic.DbServerXmlNode);
            DbUserField.Text = XmlLogic.SyncXmlValue(XmlLogic.DbUserXmlNode);
        }


        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PrinterName = PrinterNameField.Text;
            SharedLogic.SaveSettings(this);
        }

        /// <summary>
        /// Closes the popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkClicked(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }

        private void AdminModeChanged(object sender, RoutedEventArgs e)
        {
            AdminModeWarning.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            
            DbNameLabel.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            DbNameField.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;

            DbServerLabel.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            DbServerField.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;

            DbUserLabel.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            DbUserField.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
        
            DbPasswordLabel.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
            DbPasswordField.Visibility = AdminModeToggle.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
