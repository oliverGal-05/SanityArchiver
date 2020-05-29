using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SanityArchiver.Application.Models;
using ToolTip = System.Windows.Controls.ToolTip;
using SanityArchiver.DesktopUI.ViewModels;
using CheckBox = System.Windows.Controls.CheckBox;
using DataGrid = System.Windows.Controls.DataGrid;

namespace SanityArchiver.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isFaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM = new MainWindowViewModel();
            DataContext = MainWindowVM;
            _isFaded = !MainWindowVM.CaseSens;
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

            if ((DependencyObject)e.OriginalSource is System.Windows.Controls.TextBlock)
            {
                MainWindowVM.DisplayCurrentFiles((DirManagerModel)((TreeViewItem)depObj).Header);
                MainWindowVM.SelectedDirectory = (DirManagerModel)((TreeViewItem)depObj).Header;
            }
        }

        private void Select(object sender, System.Windows.RoutedEventArgs e)
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

        private void ZipSelectedButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItems = MainWindowVM.SelectedItems;
            var exampleItem = selectedItems[0];
            var selectedItemDir = System.IO.Path.GetDirectoryName(exampleItem.FullName);
            MainWindowVM.ZipFiles(selectedItemDir, selectedItems, exampleItem);
        }

        private void EncryptSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainWindowVM.SelectedItems[0];

            if (selectedItem.Name.EndsWith(".txt"))
            {
                MainWindowViewModel.EncryptTxt(selectedItem.FullName);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only .txt files can be encrypted");
            }
        }

        private void DecryptSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainWindowVM.SelectedItems[0];

            MainWindowViewModel.Decrypt(selectedItem.FullName);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            var selectedFile = (FileInfo)dg.SelectedItem;
            var attrDialog = new FileAttributeDialog(selectedFile.FullName);
            attrDialog.Show();
        }

        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                var param = e.Source as System.Windows.Controls.TextBox;

                System.Windows.Input.ICommand cmd = MainWindowVM.SearchCommand;
                if (cmd.CanExecute(param.Text))
                {
                    cmd.Execute(param.Text);
                }
            }
        }

        private void Copy_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindowVM.Copy_Move = "Copy";
        }

        private void Move_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindowVM.Copy_Move = "Move";
        }

        private void TextBlock_PreviewMouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj.GetType().Name is "TreeViewItem"))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            MainWindowVM.SelectedDirectory = (DirManagerModel)((TreeViewItem)depObj).Header;
        }

        private void PasteFiles(object sender, RoutedEventArgs e)
        {
            MainWindowVM.CopyFiles();
        }

        private void ShowSize(object sender, RoutedEventArgs e)
        {
            MainWindowVM.ShowSize();
        }

        private void SwitchCaseSensitive(object sender, RoutedEventArgs e)
        {
            MainWindowVM.SwitchCaseSensitiveness();
            _isFaded = !_isFaded;
            var img = (Image)imgbutton.Template.FindName("img", imgbutton);
            img.Opacity = _isFaded ? 0.5 : 1;
        }
    }
}