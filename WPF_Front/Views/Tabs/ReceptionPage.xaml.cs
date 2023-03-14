using Dictionary.Models.Data;
using Dictionary.Models.View;
using WPF_Front.Common;
using System.Data;
using System.Windows.Media;

namespace WPF_Front.Views.Tabs
{
    /// <summary>
    /// Interaction logic for ReceptionPage.xaml
    /// </summary>
    public partial class ReceptionPage : Page
    {
        private static List<ReceptionModel> DataBackup { get; set; }
        public ReceptionPage()
        {
            try
            {
                InitializeComponent();
                // create groupBy
                ICollectionView coll = CollectionViewSource.GetDefaultView(DGrid.ItemsSource);
                if (coll != null && coll.CanGroup == true)
                {
                    coll.GroupDescriptions.Clear();
                    coll.GroupDescriptions.Add(new PropertyGroupDescription("Order"));
                }
                SharedLogic.FetchData<ReceptionModel>(this);
                SetDataBackup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Event on row loading, changes background color if initial quantity > 100
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            if (e.Row.Item is ReceptionModel item && item.ExceedLimit)
            {
                e.Row.Background = new SolidColorBrush(Colors.BlanchedAlmond);
            }
        }

        /// <summary>
        /// Action for refresh button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SharedLogic.FetchData<ReceptionModel>(this);
            SetDataBackup();
        }

        /// <summary>
        /// Filters the dataset on when fired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            ReceptionViewModel RessourceRef = Resources["Vmodel"] as ReceptionViewModel;
            RessourceRef.Clear();
            DataBackup.Where(k => k.Order.OrderID.Contains(OrderSearch.Text)).ToList().ForEach(k => RessourceRef.Add(k));
        }

        /// <summary>
        /// Used to store the dataset before filter.
        /// </summary>
        private void SetDataBackup()
        {
            var RessourceRef = Resources["Vmodel"] as ReceptionViewModel;
            DataBackup = new();
            foreach (var data in RessourceRef)
            {
                DataBackup.Add(data);
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
            ((sender as CheckBox).DataContext as ReceptionModel).IsSelected = true;
            SharedLogic.BeginNumCellEdit(DGrid);

        }

        /// <summary>
        /// Event when line is unselected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UncheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as ReceptionModel).IsSelected = false;
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
        /// Start the selection of all object in dock panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllClicked(object sender, RoutedEventArgs e)
        {
            var parent = (sender as Button).Parent as DockPanel;
            var dContext = parent.DataContext as CollectionViewGroup; //List<ReceptionModel.ReceptionOrderModel>;
            if (dContext is not null) SharedLogic.SelectAll<ReceptionModel>(this, dContext.Items[0] as ReceptionModel);
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
        /// Extract necessary datas from source then calls PrinterConfig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            SharedLogic.Print<ReceptionModel>(this);
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

        /// <summary>
        /// Generates dataset for tests & demos -- to be deleted
        /// </summary>
        /// <param name="RessourceRef"></param>
        public static void AddDummyData(List<ReceptionModel> list)
        {
            list.Add(new ReceptionModel()
            {
                Libelle = "a",
                Reference = "00000",
                ExceedLimit = false,
                Order = new ReceptionModel.ReceptionOrderModel("12345", "AAAAA"),
                NumericProperty = 1
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "b",
                Reference = "00001",
                ExceedLimit = false,
                Order = new ReceptionModel.ReceptionOrderModel("67890", "AAAAA"),
                NumericProperty = 6
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "c",
                Reference = "00002",
                ExceedLimit = false,
                Order = new ReceptionModel.ReceptionOrderModel("12345", "BBBBB"),
                NumericProperty = 1
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "d",
                Reference = "00003",
                ExceedLimit = false,
                Order = new ReceptionModel.ReceptionOrderModel("67890", "BBBBBFGSQOFUGSODG"),
                NumericProperty = 1
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "e",
                Reference = "00004",
                ExceedLimit = false,
                Order = new ReceptionModel.ReceptionOrderModel("12345", "AAAAA"),
                NumericProperty = 1
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "f",
                Reference = "00005",
                ExceedLimit = false,
                Order = new ReceptionModel.ReceptionOrderModel("67890", "AAAAA"),
                NumericProperty = 6
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "g",
                Reference = "00006",
                ExceedLimit = true,
                Order = new ReceptionModel.ReceptionOrderModel("12345", "BBBBB"),
                NumericProperty = 0
            });
            list.Add(new ReceptionModel()
            {
                Libelle = "h",
                Reference = "00007",
                ExceedLimit = false,



                Order = new ReceptionModel.ReceptionOrderModel("67890", "BBBBB"),
                NumericProperty = 1
            });
        }
        #endregion
    }
}
