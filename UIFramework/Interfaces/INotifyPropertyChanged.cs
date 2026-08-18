using System;

namespace UIFramework.Interfaces
{
    internal interface INotifyPropertyChanged
    {
        event Action<object, UIPropertyChange> PropertyChanged;
    }
}
