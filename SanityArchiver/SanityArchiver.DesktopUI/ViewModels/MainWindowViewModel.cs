using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.AccessControl;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SanityArchiver.Application.Models;
using SanityArchiver.Application.Services;

namespace SanityArchiver.DesktopUI.ViewModels
{
    /// <summary>
    /// Some glue
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            DirManagerService = new DirManagerService();
            Drives = new ObservableCollection<DirManagerModel>(DirManagerService.GetAllDrives());
            CurrentFiles = new ObservableCollection<FileInfo>(Drives[0].DirInf.GetFiles());
            SelectedItems = new ObservableCollection<FileInfo>();
        }

        /// <summary>
        /// Gets DirInfo of Drives
        /// </summary>
        public ObservableCollection<DirManagerModel> Drives { get; private set; }

        /// <summary>
        /// Gets CurrentFiles
        /// </summary>
        public ObservableCollection<FileInfo> CurrentFiles { get; private set; }

        /// <summary>
        /// Gets files selected
        /// </summary>
        public ObservableCollection<FileInfo> SelectedItems { get; private set; }

        private DirManagerService DirManagerService { get; set; }

        /// <summary>
        /// Encrypts the selected item
        /// </summary>
        /// <param name="path">The full path of the destination of the encryption</param>
        public static void EncryptTxt(string path)
        {
            string filePath = path;
            string key = "youtubee";

            byte[] plainContent = File.ReadAllBytes(filePath);
            using (var des = new DESCryptoServiceProvider())
            {
                des.IV = Encoding.UTF8.GetBytes(key);
                des.Key = Encoding.UTF8.GetBytes(key);
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(plainContent, 0, plainContent.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(filePath, memStream.ToArray());
                    Console.WriteLine("Encrypted successfully " + filePath);
                }
            }
        }

        /// <summary>
        /// Decrypts an .enc file
        /// </summary>
        /// <param name="path">Full path of the selected item</param>
        /// <exception cref="NotImplementedException">c</exception>
        public static void Decrypt(string path)
        {
            string filePath = path;
            string key = "youtubee";

            byte[] encrypted = File.ReadAllBytes(filePath);
            using (var des = new DESCryptoServiceProvider())
            {
                des.IV = Encoding.UTF8.GetBytes(key);
                des.Key = Encoding.UTF8.GetBytes(key);
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, des.CreateDecryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(encrypted, 0, encrypted.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(filePath, memStream.ToArray());
                    Console.WriteLine("Decrypted successfully " + filePath);
                }
            }
        }

        /// <summary>
        /// Refresh to current files
        /// </summary>
        /// <param name="currentDirModel">currentDir</param>
        public void DisplayCurrentFiles(DirManagerModel currentDirModel)
        {
            CurrentFiles.Clear();
            foreach (var item in currentDirModel.DirInf.GetFiles())
            {
                CurrentFiles.Add(item);
            }
        }

        /// <summary>
        /// Compresses the selected files
        /// </summary>
        /// <param name="selectedItemDir">Directory of the selected items</param>
        /// <param name="selectedItems">List of the selected items</param>
        /// <param name="exampleItem">The first element of the selected items</param>
        public void ZipFiles(string selectedItemDir, ObservableCollection<FileInfo> selectedItems, FileInfo exampleItem)
        {
            Directory.CreateDirectory(selectedItemDir + "/temp");

            for (int i = 0; i < selectedItems.Count; i += 1)
            {
                var selectedItem = selectedItems[i];
                File.Copy(selectedItem.FullName, selectedItemDir + "/temp/" + selectedItem.Name, true);
                File.Delete(selectedItem.Name);
            }

            string startPath = selectedItemDir + "/temp";
            string zipPath = selectedItemDir + "/" + exampleItem.Name + ".zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);

            Directory.Delete(startPath, true);
        }
    }
}
