using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WordCountingApp.Models;
using System.Windows.Documents;

namespace WordCountingApp.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region RelayCommands
        public RelayCommand SelectFileClickedCommand
        {
            get;
            private set;
        }
        public RelayCommand CountWordsClickedCommand
        {
            get;
            private set;
        }
        #endregion RelayCommands

        #region fields
        private ObservableCollection<RankedWordCountItem> wordCountItems;
        private string selectedFilePath;
        private bool isLoading;
        #endregion fields

        #region Properties
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSelectFileButtonEnabled));
                    OnPropertyChanged(nameof(IsCountWordsButtonEnabled));
                }
            }
        }

        public FlowDocument SelectedFile { get; set; }

        public string SelectedFilePath
        {
            get => selectedFilePath;
            set
            {
                if (selectedFilePath != value)
                {
                    selectedFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSelectFileButtonEnabled
        {
            get => !IsLoading;
        }

        public bool IsCountWordsButtonEnabled
        {
            get => !IsLoading;
        }
        #endregion Properties

        public ObservableCollection<RankedWordCountItem> WordCountItems
        {
            get => wordCountItems;
            set => wordCountItems = value;
        }

        public MainWindowViewModel()
        {
            SelectFileClickedCommand = new RelayCommand(SelectFileClicked, () => !IsLoading);
            CountWordsClickedCommand = new RelayCommand(CountWordsClicked, () => !IsLoading);

            WordCountItems = new ObservableCollection<RankedWordCountItem>();
            WordCountItems.CollectionChanged += WordCountItems_CollectionChanged;

        }

        private void SelectFileClicked()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            if ((bool)openFileDialog.ShowDialog())
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    SelectedFilePath = openFileDialog.FileName;
                }
            }
        }

        private void CountWordsClicked()
        {
            DisplayRankedWordCounts();
        }

        private async void DisplayRankedWordCounts()
        {
            IsLoading = true;
            WordCountItems.Clear();

            Dictionary<string,int> wordCounts = new Dictionary<string,int>(await Task.Run(
                () => WordCounting.FileWordCounter.CountWords(SelectedFilePath,StringComparer.CurrentCultureIgnoreCase)));

            int index = 0;
            foreach (KeyValuePair<string,int> wordCount in wordCounts.OrderByDescending(x => x.Value))
            {
                index++;
                WordCountItems.Add(new RankedWordCountItem(index, wordCount.Key, wordCount.Value));
            }
            IsLoading = false;
        }

        #region ObservableCollection CollectionChanged EventHandler
        private void WordCountItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(WordCountItems));
        }
        #endregion ObservableCollection CollectionChanged EventHandler

        #region INotifyPropertyChanged Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
