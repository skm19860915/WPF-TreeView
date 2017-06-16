using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WPFTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var drive in Directory.GetLogicalDrives())
            {
                // create a new item for it.
                var item = new TreeViewItem()
                {
                    // set the header
                    Header = drive,
                    // and the full path
                    Tag = drive,
                };
                
                // add a dummy item
                item.Items.Add(null);
                //listen out for item being expanded
                item.Expanded += Folder_Expanded;
                //add it to the main tree-view
                FolderView.Items.Add(item);
            }
        }
        #region Folder Expanded
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            // if the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;
            // clear dummy data
            item.Items.Clear();
            // get full path
            var fullPath = (string)item.Tag;
            #endregion
            #region Get Folders
            // crate blank list for directories
            var directories = new List<string>();
            // try and get directories from folder
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch
            {

            }

            // for each directory..
            directories.ForEach(directoryPath => 
            {
                // create directory item
                var subItem = new TreeViewItem()
                {
                    Header = GetFileFolderName(directoryPath),
                    Tag = directoryPath
                };

                subItem.Items.Add(null);

                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });
            #endregion
            #region Get Files
            // crate blank list for files
            var files = new List<string>();
            // try and get files from folder
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch
            {

            }

            // for each files..
            files.ForEach(filePath =>
            {
                // create file item
                var subItem = new TreeViewItem()
                {
                    Header = GetFileFolderName(filePath),
                    Tag = filePath
                };

                item.Items.Add(subItem);
            });
            #endregion
        }
        #endregion
        #region helpers
        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            var normalizedPath = path.Replace("/", "\\");
            var lastIndex = normalizedPath.LastIndexOf('\\');
            if (lastIndex <= 0)
                return path;

            return path.Substring(lastIndex + 1);
        }
        #endregion
    }
} 
