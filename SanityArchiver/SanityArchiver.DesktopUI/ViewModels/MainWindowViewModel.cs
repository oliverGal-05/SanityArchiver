using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SanityArchiver.Application.Models;
using SanityArchiver.Application.Services;

namespace SanityArchiver.DesktopUI.ViewModels
{
    /// <summary>
    /// Some glue
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RelayCommand _seachCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            DirManagerService = new DirManagerService();
            Drives = new ObservableCollection<DirManagerModel>(DirManagerService.GetAllDrives());
            CurrentFiles = new ObservableCollection<FileInfo>(Drives[0].DirInf.GetFiles());
            SelectedItems = new ObservableCollection<FileInfo>();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or Sets the string containing the result of search.
        /// </summary>
        public string SearchResult { get; set; }

        /// <summary>
        /// Gets the command that search for file from user input.
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                if (_seachCommand == null)
                {
                    _seachCommand = new RelayCommand(SearchInput, CanSearch);
                }

                return _seachCommand;
            }
        }

        /// <summary>
        /// Gets DirInfo of Drives
        /// </summary>
        public ObservableCollection<DirManagerModel> Drives { get; private set; }

        /// <summary>
        /// Gets CurrentFiles
        /// </summary>
        public ObservableCollection<FileInfo> CurrentFiles { get; private set; }

        /// <summary>
        /// Gets files selected
        /// </summary>
        public ObservableCollection<FileInfo> SelectedItems { get; private set; }

        private DirManagerService DirManagerService { get; set; }

        /// <summary>
        /// Refresh to current files
        /// </summary>
        /// <param name="currentDirModel">currentDir</param>
        public void DisplayCurrentFiles(DirManagerModel currentDirModel)
        {
            CurrentFiles.Clear();

            // TODO unauthorized access
            foreach (var item in currentDirModel.DirInf.GetFiles())
            {
                CurrentFiles.Add(item);
            }
        }

        /// <summary>
        /// Compresses the selected files
        /// </summary>
        /// <param name="selectedItemDir">Directory of the selected items</param>
        /// <param name="selectedItems">List of the selected items</param>
        /// <param name="exampleItem">The first element of the selected items</param>
        public void ZipFiles(string selectedItemDir, ObservableCollection<FileInfo> selectedItems, FileInfo exampleItem)
        {
            System.IO.Directory.CreateDirectory(selectedItemDir + "/temp");

            for (int i = 0; i < selectedItems.Count; i += 1)
            {
                var selectedItem = selectedItems[i];
                File.Copy(selectedItem.FullName, selectedItemDir + "/temp/" + selectedItem.Name, true);
                File.Delete(selectedItem.Name);
            }

            string startPath = selectedItemDir + "/temp";
            string zipPath = selectedItemDir + "/" + exampleItem.Name + ".zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);

            System.IO.Directory.Delete(startPath, true);
        }

        private bool CanSearch(object parameter)
        {
            if (!string.IsNullOrEmpty((string)parameter) && ((string)parameter).Length > 2)
            {
                return true;
            }

            return false;
        }

        private void SearchInput(object parameter)
        {
            // implement searching functions here.

            // TODO: set the result string to the actual result. This is just a placeholder.
            SearchResult = "searching for " + parameter as string;
            NotifyPropertyChanged("SearchResult");
        }

        private void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
