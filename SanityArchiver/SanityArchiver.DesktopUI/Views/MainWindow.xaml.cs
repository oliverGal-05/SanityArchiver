using System.Windows;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media;
using SanityArchiver.DesktopUI.ViewModels;
using SanityArchiver.Application.Models;
using CheckBox = System.Windows.Controls.CheckBox;

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

        private void Select(object sender, RoutedEventArgs e)
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

        private void ZipSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = MainWindowVM.SelectedItems;
            var exampleItem = selectedItems[0];
            var selectedItemDir = Path.GetDirectoryName(exampleItem.FullName);

            MainWindowVM.ZipFiles(selectedItemDir, selectedItems, exampleItem);
        }

        private void EncryptSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainWindowVM.SelectedItems[0];
            var selectedItemDir = Path.GetDirectoryName(selectedItem.FullName);

            if (selectedItem.Name.EndsWith(".txt"))
            {
                MainWindowViewModel.EncryptTxt(selectedItemDir, selectedItem.FullName);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only .txt files can be encrypted");
            }
        }

        private void DecryptSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainWindowVM.SelectedItems[0];
            var selectedItemDir = Path.GetDirectoryName(selectedItem.FullName);

            MainWindowViewModel.Decrypt(selectedItemDir, selectedItem.FullName);
        }
    }
}