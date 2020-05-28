using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanityArchiver.Application.Models
{
    /// <summary>
    /// Model for Dir
    /// </summary>
    public class DirManagerModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirManagerModel"/> class.
        /// </summary>
        /// <param name="fullName">Path</param>
        public DirManagerModel(string fullName)
        {
            DirInf = new DirectoryInfo(fullName);
        }

        /// <summary>
        /// Gets Name of Dir
        /// </summary>
        public string Name => DirInf.Name;

        /// <summary>
        /// Gets FullName of Dir
        /// </summary>
        public string FullName => DirInf.FullName;

        /// <summary>
        /// Gets SubDirs of CurrentDir
        /// </summary>
        public List<DirManagerModel> Directories
        {
            get
            {
                List<DirManagerModel> directoryInfos = new List<DirManagerModel>();
                if (DirInf != null)
                {
                    try
                    {
                        foreach (var item in DirInf.GetDirectories())
                        {
                            try
                            {
                                DirManagerModel currentDirectory = new DirManagerModel(item.FullName);
                                if (currentDirectory.DirInf.Exists)
                                {
                                    directoryInfos.Add(new DirManagerModel(item.FullName));
                                }
                            }
                            catch (UnauthorizedAccessException)
                            {
                            }
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return null;
                    }
                }

                return directoryInfos;
            }
        }

        /// <summary>
        /// Gets or sets dirInf
        /// </summary>
        public DirectoryInfo DirInf { get; set; }

        /// <summary>
        /// niceone
        /// </summary>
        /// <returns>Size of the files and folders in current folder</returns>
        public long GetSize()
        {
            long size = 0;
            try
            {
                FileInfo[] fis = DirInf.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
            }
            catch (UnauthorizedAccessException)
            {
            }

            try
            {
                foreach (var di in Directories)
                {
                    size += di.GetSize();
                }
            }
            catch (UnauthorizedAccessException)
            {
            }

            return size;
        }

        /// <summary>
        /// Calculat the folder's size in int
        /// </summary>
        /// <param name="actualSize">size of folder in KB</param>
        /// <returns>string holding size with proper unit</returns>
        public string GetSizeInString(long actualSize)
        {
            int sizeInInt = 0;
            int dividerKB = 1024;
            int dividerMB = dividerKB * dividerKB;
            int dividerGB = dividerMB * dividerKB;

            string result = " ";
            if (actualSize > dividerGB)
            {
                sizeInInt = (int)(actualSize / dividerGB);
                result = sizeInInt.ToString() + " GB";
            }
            else if (actualSize > dividerMB)
            {
                sizeInInt = (int)(actualSize / dividerMB);
                result = sizeInInt.ToString() + " MB";
            }
            else if (actualSize > dividerKB)
            {
                sizeInInt = (int)(actualSize / dividerKB);
                result = sizeInInt.ToString() + " KB";
            }
            else
            {
                result = actualSize.ToString() + " byte";
            }

            return result;
        }
    }
}
