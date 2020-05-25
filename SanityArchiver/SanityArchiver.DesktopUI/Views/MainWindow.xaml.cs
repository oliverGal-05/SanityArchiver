using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
    }
}
