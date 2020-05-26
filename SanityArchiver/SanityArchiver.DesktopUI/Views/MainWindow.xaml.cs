using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using SanityArchiver.DesktopUI.ViewModels;
using SanityArchiver.Application.Models;

namespace SanityArchiver.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// save the clicked path for copy
        /// </summary>
        private string _pathToCopy;

        /// <summary>
        /// copy selected or not
        /// </summary>
        private bool _copyStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM = new MainWindowViewModel();
            DataContext = MainWindowVM;
        }

        /// <summary>
        /// Gets or sets MainVM
        /// </summary>
        public MainWindowViewModel MainWindowVM { get; set; }

        private void TreeView_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj.GetType().Name is "TreeViewItem"))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            if ((DependencyObject)e.OriginalSource is TextBlock)
            {
                MainWindowVM.DisplayCurrentFiles((DirManagerModel)((TreeViewItem)depObj).Header);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj.GetType().Name is "DataGridRow"))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            if (((CheckBox)((DependencyObject)e.OriginalSource)).IsChecked == true)
            {
                MainWindowVM.SelectedItems.Add((FileInfo)((DataGridRow)depObj).Item);
            }
            else
            {
                MainWindowVM.SelectedItems.Remove((FileInfo)((DataGridRow)depObj).Item);
            }
        }

        private void Copy_Clicked(object sender, RoutedEventArgs e)
        {
            _copyStatus = true;
        }

        private void TextBlock_PreviewMouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj.GetType().Name is "TreeViewItem"))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            _pathToCopy = ((DirManagerModel)((TreeViewItem)depObj).Header).FullName;
        }

        private void PasteFiles(object sender, RoutedEventArgs e)
        {
            if (_copyStatus == true)
            {
                MainWindowVM.CopyFiles(MainWindowVM.SelectedItems, _pathToCopy);
                _copyStatus = false;
            }
        }
    }
}
