using System.Collections.Generic;
using System.IO;
using SanityArchiver.Application.Models;

namespace SanityArchiver.Application.Services
{
    /// <summary>
    /// GetDrives
    /// </summary>
    public class DirManagerService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirManagerService"/> class.
        /// </summary>
        /// <returns>All drives in <see cref="DirManagerModel"/></returns>
        public List<DirManagerModel> GetAllDrives()
        {
            List<DirManagerModel> dirManagerModels = new List<DirManagerModel>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    dirManagerModels.Add(new DirManagerModel(drive.RootDirectory.FullName));
                }
            }

            return dirManagerModels;
        }
    }
}
