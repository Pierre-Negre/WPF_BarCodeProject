using Dictionary.Models.Data;
using System.Collections.ObjectModel;

namespace Dictionary.Models.View
{
    /// <summary>
    /// Base viewModel for data binding
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseViewModel<T> : ObservableCollection<T> where T : BaseModel
    {
    }
}
