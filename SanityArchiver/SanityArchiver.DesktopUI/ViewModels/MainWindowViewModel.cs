using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SanityArchiver.Application.Models;
using SanityArchiver.Application.Services;

namespace SanityArchiver.DesktopUI.ViewModels
{
    /// <summary>
    /// Some glue
    /// </summary>
    public class MainWindowViewModel
    {
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
            foreach (var item in currentDirModel.DirInf.GetFiles())
            {
                CurrentFiles.Add(item);
            }
        }

        /// <summary>
        /// Copy selected files
        /// </summary>
        /// <param name="filesToCopy">selected items</param>
        /// <param name="path">the path to copy to</param>
        public void CopyFiles(ObservableCollection<FileInfo> filesToCopy, string path)
        {
            string toPathOrig = path;
            foreach (var item in filesToCopy)
            {
                string name = item.Name;

                // string ext = item.Extension;

                string toDestinPath = toPathOrig + name;
                item.CopyTo(toDestinPath);
            }
        }
    }
}
