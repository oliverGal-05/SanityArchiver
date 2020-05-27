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
                            directoryInfos.Add(new DirManagerModel(item.FullName));
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
    }
}
