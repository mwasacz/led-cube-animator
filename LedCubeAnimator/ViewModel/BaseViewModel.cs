using GalaSoft.MvvmLight;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        public BaseViewModel(UndoManager undo)
        {
            Undo = undo;
        }

        public UndoManager Undo { get; }

        protected void Set(object obj, string name, object newValue)
        {
            Undo.Set(obj, name, newValue);
        }

        /*protected bool SetObsevable<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null) where T : INotifyPropertyChanged
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) // ToDo: field == newValue
            {
                return false;
            }

            if (field != null)
            {
                field.PropertyChanged -= Observable_PropertyChanged;
            }
            if (newValue != null)
            {
                newValue.PropertyChanged += Observable_PropertyChanged;
            }
            field = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }

        private void Observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHandler?.Invoke(this, new NestedPropertyChangedEventArgs(sender, e));
        }

        protected bool SetCollection<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null) where T : INotifyCollectionChanged
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) // ToDo: field == newValue
            {
                return false;
            }

            if (field != null)
            {
                field.CollectionChanged -= Collection_CollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += Collection_CollectionChanged;
            }
            field = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChangedHandler?.Invoke(this, new NestedCollectionChangedEventArgs(sender, e));
        }*/
    }
}
