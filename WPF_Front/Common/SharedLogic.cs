using DAL;
using Dictionary.Enums;
using Dictionary.Models.Data;
using Dictionary.Models.Print;
using Dictionary.Models.View;
using WPF_Front.Views.Tabs;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Threading;

namespace WPF_Front.Common
{
    /// <summary>
    /// Shared logic between all views
    /// </summary>
    internal static class SharedLogic
    {
        /// <summary>
        /// waiting time to allow the popup to exist
        /// </summary>
        private static TimeSpan WaitTime => new(0, 0, 0, 0, 200);

        /// <summary>
        /// Keyboard Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void KeyLogic(KeyEventArgs e, DataGrid grid)
        {
            var objectTreated = new List<BaseModel>();
            foreach (var cell in grid.SelectedCells)
            {
                var k = cell.Item as BaseModel;
                if (k is not null && objectTreated.IndexOf(k) == -1)
                {
                    switch (e.Key)
                    {
                        case Key.Space:
                            k.IsSelected = !k.IsSelected;
                            BeginNumCellEdit(grid);
                            break;
                        case Key.Add:
                            if (k.NumericProperty < 100 && k.IsSelected)
                                k.NumericProperty++;
                            break;
                        case Key.Subtract:
                            if (k.NumericProperty > 0 && k.IsSelected)
                                k.NumericProperty--;
                            break;
                    }
                    objectTreated.Add(k);
                }
            }
        }

        /// <summary>
        /// Focus the num cell of the selected row and start the edit
        /// </summary>
        /// <param name="grid"></param>
        internal static void BeginNumCellEdit(DataGrid grid)
        {
            if (grid.SelectedCells.Count == 6)
            {
                grid.CurrentCell = grid.SelectedCells[4];
                grid.BeginEdit();
            }
        }

        /// <summary>
        /// Changes IsSelected of all item in a group
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elem"></param>
        /// <param name="item"></param>
        internal async static void SelectAll<T>(FrameworkElement elem, T item) where T : BaseModel
        {
            BaseViewModel<T>? RessourceRef = elem.Resources["Vmodel"] as BaseViewModel<T>;

            foreach (var data in RessourceRef)
            {
                switch (elem)
                {
                    case ReceptionPage:

                        if ((data as ReceptionModel).Order.Equals((item as ReceptionModel).Order))
                        {
                            data.IsSelected = true;
                        }
                        break;

                }
            }
        }

        /// <summary>
        /// Calls PrintLogic to cancel ongoing print commands
        /// </summary>
        /// <param name="elem"></param>
        internal async static void CancelPrint(FrameworkElement elem)
        {
            // getting the popup
            Popup popup = FindChild<Popup>(elem, "myPopup");
            TextBlock popupText = FindChild<TextBlock>(popup.Child, "myPopupText");
            Button popupButton = FindChild<Button>(popup.Child, "OkButton");

            // setting the popup content
            popup.IsOpen = true;
            popupButton.Visibility = Visibility.Hidden;
            popupText.Text = "Envoie de la commande d'arret";

            // displays the popup at least a little
            var timer = new PeriodicTimer(WaitTime);
            await timer.WaitForNextTickAsync();

            var result = PrinterLogic.Cancel();

            popupButton.Visibility = Visibility.Visible;
            switch (result)
            {
                case 1801:
                    popupText.Text = "Imprimante non trouvée";
                    return;
                default:
                    popupText.Text = "Impressions stoppées";
                    return;
            }
        }

        /// <summary>
        /// Extract necessary datas from source then calls PrinterConfig
        /// </summary>
        /// <param name="elem"></param>
        internal async static void Print<T>(FrameworkElement elem) where T : BaseModel
        {
            // getting the popup
            Popup popup = FindChild<Popup>(elem, "myPopup");
            TextBlock popupText = FindChild<TextBlock>(popup.Child, "myPopupText");
            Button popupButton = FindChild<Button>(popup.Child, "OkButton");

            // setting the popup content
            popup.IsOpen = true;
            popupButton.Visibility = Visibility.Hidden;
            popupText.Text = "Impression en cours";

            // displays the popup at least a little
            var timer = new PeriodicTimer(WaitTime);
            await timer.WaitForNextTickAsync();

            BaseViewModel<T>? RessourceRef = elem.Resources["Vmodel"] as BaseViewModel<T>;
            var dataToPrint = new List<PrintInfos>();
            if (RessourceRef is not null)
            {
                foreach (var part in RessourceRef)
                {
                    if (part.IsSelected)
                    {
                        dataToPrint.Add(new PrintInfos
                        {
                            TextLine = part.Libelle,
                            BarCodeLine = part.Reference,
                            Emplacement = part.Emplacement,
                            PrintQty = part.NumericProperty
                        });
                    }
                }
            }
            var result = PrinterLogic.Print(dataToPrint);

            popupButton.Visibility = Visibility.Visible;
            switch (result)
            {
                case -1:
                    popupText.Text = "Aucune donnée séléctionnée";
                    return;
                case 1801:
                    popupText.Text = "Imprimante non trouvée";
                    return;
                default:
                    popupText.Text = "Impression envoyée avec succès";
                    return;
            }
        }

        /// <summary>
        /// Increment the NumValue for the item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void UpNumValue<T>(DataGrid grid, string id) where T : BaseModel
        {
            var objectTreated = new List<BaseModel>();
            foreach (var cell in grid.SelectedCells)
            {
                var item = cell.Item as BaseModel;
                if (item is not null && objectTreated.IndexOf(item) == -1)
                {
                    if (item.IsSelected && item.NumericProperty < 100 && item.Reference == id)
                    {
                        item.NumericProperty++;
                    }
                    objectTreated.Add(item);
                }
            }
        }

        /// <summary>
        /// Decrement the NumValue for the item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void DownNumValue<T>(DataGrid grid, string id) where T : BaseModel
        {
            var objectTreated = new List<BaseModel>();
            foreach (var cell in grid.SelectedCells)
            {
                var item = cell.Item as BaseModel;
                if (item is not null && objectTreated.IndexOf(item) == -1)
                {
                    if (item.IsSelected && item.NumericProperty > 0 && item.Reference == id)
                    {
                        item.NumericProperty--;
                    }
                    objectTreated.Add(item);
                }
            }
        }

        /// <summary>
        /// Calls DAL & sort gathered data for the specified page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elem"></param>
        internal async static void FetchData<T>(FrameworkElement elem) where T : BaseModel
        {
            Popup popup = null;
            TextBlock popupText = null;
            Button popupButton = null;
            var timer = new PeriodicTimer(WaitTime);

            // getting the popup
            popup = FindChild<Popup>(elem, "myPopup");
            if (popup != null)
            {
                popupText = FindChild<TextBlock>(popup.Child, "myPopupText");
                popupButton = FindChild<Button>(popup.Child, "OkButton");

                // setting the popup content
                popup.IsOpen = true;
                popupButton.Visibility = Visibility.Hidden;
                popupText.Text = "Récuperation des données ...";
                popupText.VerticalAlignment = VerticalAlignment.Center;

                // displays the popup at least a little
                await timer.WaitForNextTickAsync();
            }

            string DataSource = XmlLogic.SyncXmlValue(XmlLogic.DbServerXmlNode);
            string DataBase = XmlLogic.SyncXmlValue(XmlLogic.DbNameXmlNode);
            Exception? ex = null;
            BaseViewModel<T>? RessourceRef = elem.Resources["Vmodel"] as BaseViewModel<T>;
            List<T> datas = new();
            if (RessourceRef is not null)
            {
                // Changes the request base on calling page
                switch (elem)
                {
                    case ReceptionPage:
                        RessourceRef.Clear();
                        datas = SqlLiaison.GetData<T>(ViewsEnum.Reception, DataSource, DataBase, XmlLogic.SyncXmlValue(XmlLogic.DbUserXmlNode), PasswordLogic.Decode(XmlLogic.SyncXmlValue(XmlLogic.DbPasswordXmlNode)), out ex).ToList();

                        if (ex is not null)
                        {
                            XmlLogic.SyncXmlValue(XmlLogic.DbPasswordXmlNode, Properties.Settings.Default.DBPassword);
                            datas = SqlLiaison.GetData<T>(ViewsEnum.Reception, DataSource, DataBase, XmlLogic.SyncXmlValue(XmlLogic.DbUserXmlNode), PasswordLogic.Decode(XmlLogic.SyncXmlValue(XmlLogic.DbPasswordXmlNode)), out ex).ToList();
                        }
                        try
                        {
                            foreach (var data in datas)
                            {
                                var collBla = SqlLiaison.GetData<PartsModel>(ViewsEnum.Parts, DataSource, DataBase, XmlLogic.SyncXmlValue(XmlLogic.DbUserXmlNode), PasswordLogic.Decode(XmlLogic.SyncXmlValue(XmlLogic.DbPasswordXmlNode)), out ex, data.Reference).ToList();
                                var bla = collBla.First();
                                data.Emplacement = bla.Emplacement;
                            }
                        }
                        catch { }

                        if (!datas.Any()) ReceptionPage.AddDummyData(datas as List<ReceptionModel>); // TESTS & DEMOS ONLY
                        break;
                    case PartsPage:
                        TextBox RefSearchBox = FindChild<TextBox>(elem, "RefSearchBox");
                        TextBox LibelleSearchBox = FindChild<TextBox>(elem, "LibelleSearchBox");

                        var reftext = RefSearchBox.Text;
                        var NewIndincatorIndex = reftext.IndexOf("-N");
                        var reffiltered = NewIndincatorIndex != -1 ? reftext.Remove(NewIndincatorIndex,2) : reftext;
                        datas = SqlLiaison.GetData<T>(ViewsEnum.Parts, DataSource, DataBase, XmlLogic.SyncXmlValue(XmlLogic.DbUserXmlNode), PasswordLogic.Decode(XmlLogic.SyncXmlValue(XmlLogic.DbPasswordXmlNode)), out ex, reffiltered, LibelleSearchBox.Text).ToList();
                        break;
                }
                if (datas is not null)
                {
                    foreach (var data in datas)
                    {
                        // prevent duplicating searches for PartsPage
                        if (typeof(T) is PartsModel)
                        {
                            if ((RessourceRef as BaseViewModel<PartsModel>).Where(k => k.Search.Equals((data as PartsModel).Search) && k.Reference == data.Reference).Any())
                                break;
                        }
                        RessourceRef.Add(data);
                    }

                    // give time for datagrid to refresh
                    timer = new PeriodicTimer(WaitTime);
                    await timer.WaitForNextTickAsync();
                }
            }
            if (popup != null)
            {
                if (ex != null)
                {
                    var message = "Erreur lors de la requête" + Environment.NewLine;
                    if (ex.InnerException is null)
                        message += ex.Message;
                    else
                        message += ex.InnerException.Message;
                    popupText.Text = message;
                    popupText.VerticalAlignment = VerticalAlignment.Top;
                    popupButton.Visibility = Visibility.Visible;
                }
                else popup.IsOpen = false;
            }
        }

        /// <summary>
        /// Retrieves fields value and calls XML sync methods
        /// </summary>
        /// <param name="elem"></param>
        internal static void SaveSettings(FrameworkElement elem)
        {
            // getting the popup
            Popup popup = FindChild<Popup>(elem, "myPopup");
            TextBlock popupText = FindChild<TextBlock>(popup.Child, "myPopupText");
            Button popupButton = FindChild<Button>(popup.Child, "OkButton");

            XmlLogic.SyncXmlValue(XmlLogic.PrinterNameXmlNode, FindChild<TextBox>(elem, "PrinterNameField").Text);
            XmlLogic.SyncXmlValue(XmlLogic.DbNameXmlNode, FindChild<TextBox>(elem, "DbNameField").Text);
            XmlLogic.SyncXmlValue(XmlLogic.DbServerXmlNode, FindChild<TextBox>(elem, "DbServerField").Text);
            XmlLogic.SyncXmlValue(XmlLogic.DbUserXmlNode, FindChild<TextBox>(elem, "DbUserField").Text);
            if(string.IsNullOrWhiteSpace(FindChild<PasswordBox>(elem, "DbPasswordField").Password)) XmlLogic.SyncXmlValue(XmlLogic.DbPasswordXmlNode, PasswordLogic.Encode(FindChild<PasswordBox>(elem, "DbPasswordField").Password));
            XmlLogic.SyncXmlValue(XmlLogic.PrinterModelXmlNode, "Zebra");

            if (popupText != null && popupButton != null)
            {
                popup.IsOpen = true;
                popupButton.Visibility = Visibility.Visible;
                popupText.Text = "Paramètres sauvegardés";
            }
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <param name="childName"></param>
        /// <returns></returns>
        private static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                if (child is not T)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }        
    }
}
