using Dictionary.Models.Data;
using Dictionary.Models.View;
using WPF_Front.Common;

namespace WPF_Front.Views.Tabs
{
    /// <summary>
    /// Interaction logic for PartsPage.xaml
    /// </summary>
    public partial class PartsPage : Page
    {
        private PartsViewModel? RessourceRef => this.Resources["Vmodel"] as PartsViewModel;

        public PartsPage()
        {
            InitializeComponent();
            // create groupBy
            ICollectionView coll = CollectionViewSource.GetDefaultView(DGrid.ItemsSource);
            if (coll != null && coll.CanGroup == true)
            {
                coll.GroupDescriptions.Clear();
                coll.GroupDescriptions.Add(new PropertyGroupDescription("Search"));
            }
        }

        /// <summary>
        /// control search buttons logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (RessourceRef is not null)
            {
                if (sender is not Button obj) return;
                switch (obj.Tag.ToString())
                {
                    case "ResetSearch":
                        RessourceRef.Clear();
                        RefSearchBox.Text = string.Empty;
                        LibelleSearchBox.Text = string.Empty;
                        ResetButton.Visibility = Visibility.Hidden;
                        AddButton.Visibility = Visibility.Hidden;
                        break;
                    case "AddSearch":
                        SharedLogic.FetchData<PartsModel>(this);
                        break;
                    case "NewSearch":
                        RessourceRef.Clear();
                        SharedLogic.FetchData<PartsModel>(this);
                        AddButton.Visibility = Visibility.Visible;
                        ResetButton.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        #region Common Page Methods        

        /// <summary>
        /// Event when line is selected => calls cell edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as PartsModel).IsSelected = true;
            SharedLogic.BeginNumCellEdit(DGrid);
        }

        /// <summary>
        /// Event when line is unselected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UncheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as PartsModel).IsSelected = false;
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

        /// <summary>
        /// Get the row's object then call the increment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpNumValue(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString();
            SharedLogic.UpNumValue<PartsModel>(DGrid, id);
        }

        /// <summary>
        /// Get the row's object then call the decrement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownNumValue(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString();
            SharedLogic.DownNumValue<PartsModel>(DGrid, id);
        }

        /// <summary>
        /// Fire when cancelling prints
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            SharedLogic.CancelPrint(this);
        }

        /// <summary>
        /// Extract necessary datas from source then calls PrinterConfig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            SharedLogic.Print<PartsModel>(this);
        }

        /// <summary>
        /// Keyboard Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            SharedLogic.KeyLogic(e, DGrid);
        }        
        #endregion
    }
}