using System.IO;

namespace SanityArchiver.DesktopUI.ViewModels
{
    /// <summary>
    /// Handles properties and methods binded to FileAttributeDialog view.
    /// </summary>
    public class FileAttributeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttributeViewModel"/> class.
        /// </summary>
        /// <param name="file">FileInfo</param>
        public FileAttributeViewModel(string file)
        {
            File = new FileInfo(file);
            FilteredFile = new MyFile(File);
        }

        /// <summary>
        /// Gets or sets File.
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// Gets or Sets FilteredFile.
        /// </summary>
        public MyFile FilteredFile { get; set; }

        /// <summary>
        /// Inner helper class to store different File properties and attributes.
        /// </summary>
        public class MyFile
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MyFile"/> class.
            /// </summary>
            /// <param name="file">Used to initialize properties.</param>
            public MyFile(FileInfo file)
            {
                Name = Path.GetFileNameWithoutExtension(file.FullName);
                Extension = file.Extension;
                Hidden = file.Attributes.HasFlag(FileAttributes.Hidden);
            }

            /// <summary>
            /// Gets the Name.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets Extension
            /// </summary>
            public object Extension { get; private set; }

            /// <summary>
            /// Gets or sets a value indicating whether is hidden or not.
            /// </summary>
            public bool Hidden { get; set; }
        }
    }
}
