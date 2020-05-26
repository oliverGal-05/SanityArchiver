using System.Windows;
using SanityArchiver.DesktopUI.ViewModels;

namespace SanityArchiver.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for FileAttributeDialog.xaml
    /// </summary>
    public partial class FileAttributeDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttributeDialog"/> class.
        /// </summary>
        /// <param name="path">Full path of file.</param>
        public FileAttributeDialog(string path)
        {
            InitializeComponent();
            FileAttributeViewModel vm = new FileAttributeViewModel(path);
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = new System.Action(Close);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
