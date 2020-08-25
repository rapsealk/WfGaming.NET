using System.ComponentModel;

namespace WfGaming.Interfaces
{
    interface IViewModel
    {
        void OnPropertyChanged(PropertyChangedEventArgs e);
    }
}
