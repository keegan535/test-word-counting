using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WordCountingApp.Models;

namespace WordCountingApp.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<RankedWordCountItem> wordCountItems;

        public ObservableCollection<RankedWordCountItem> WordCountItems
        {
            get => wordCountItems;
            set => wordCountItems = value;
        }

        public MainWindowViewModel()
        {
            WordCountItems = new ObservableCollection<RankedWordCountItem>();
        }

        #region INotifyPropertyChanged Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
