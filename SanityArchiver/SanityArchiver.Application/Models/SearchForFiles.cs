using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            Regex searchTerm = new Regex(filename);
            result = RecursiveSearchFiles(path, searchTerm, result);
            return result;
        }

        private static List<string> RecursiveSearchFiles(string path, Regex searchTerm, List<string> files)
        {
            try
            {
                var fileList = Directory.GetFiles(path);
                var queryMatchingFiles =
                    from file in fileList
                    let fileText = Path.GetFileNameWithoutExtension(file)
                    let matches = searchTerm.Matches(fileText)
                    where matches.Count > 0
                    select file;

                foreach (var file in queryMatchingFiles)
                {
                    files.Add(file);
                }

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => RecursiveSearchFiles(s, searchTerm, files));
            }
            catch (UnauthorizedAccessException)
            {
                // not allowed directory, continue loop.
            }

            return files;
        }
    }
}
