using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SanityArchiver.Application.Models
{
    /// <summary>
    /// Contains methods for filesearching.
    /// </summary>
    public class SearchForFiles
    {
        /// <summary>
        /// Search for files in given directory and subdirectories.
        /// </summary>
        /// <param name="path">The path of Directory.</param>
        /// <param name="filename">Name of the file</param>
        /// <returns>A list of string containing full path of found files.</returns>
        public static List<string> SearchFiles(string path, string filename)
        {
            var result = new List<string>();
            result = RecursiveSearchFiles(path, filename, result);
            return result;
        }

        private static List<string> RecursiveSearchFiles(string path, string filename, List<string> files)
        {
            try
            {
                Directory.GetFiles(path)
                    .Where(f => filename.ToLower().Equals(Path.GetFileName(f).ToLower()))
                    .ToList()
                    .ForEach(f => files.Add(f));

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => RecursiveSearchFiles(s, filename, files));
            }
            catch (UnauthorizedAccessException)
            {
                // not allowed directory, continue loop.
            }

            return files;
        }
    }
}
