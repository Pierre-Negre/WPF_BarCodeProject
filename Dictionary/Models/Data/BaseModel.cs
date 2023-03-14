using System.ComponentModel;

namespace Dictionary.Models.Data
{
    /// <summary>
    /// Base business object, with property changed events for databinding
    /// </summary>
    public class BaseModel : INotifyPropertyChanged
    {

        private bool isSelected;
        private int numericProperty;
        public string Reference { get; set; }
        public string Libelle { get; set; }
        public string Emplacement { get; set; }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        public int NumericProperty
        {
            get { return numericProperty; }
            set { numericProperty = value; OnPropertyChanged("NumericProperty"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public override bool Equals(object? obj)
        {
            return obj is BaseModel baseModel && baseModel.Libelle == Libelle && baseModel.Reference == Reference;
        }
    }
}
