using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ScanerUI
{
    public class DirectoryScanner
    {
        public event EventHandler ScanCompleted;

        private Queue<Item> directories;

        private string path;

        public DirectoryScanner(string path, Queue<Item> directories)
        {
            this.path = path;
            this.directories = directories;
        }

        public void StartScan()
        {
            lock (((ICollection)directories).SyncRoot)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    throw new ApplicationException("There is no such directory");
                }

                var root = new Directory(path, null, 0);
                directories.Enqueue(root);
                Monitor.Pulse(((ICollection)directories).SyncRoot);
                FillDirectoriesList(root);
            }

            if (ScanCompleted != null)
            {
                ScanCompleted(this, EventArgs.Empty);
            }            
        }

        private void FillDirectoriesList(Directory directory)
        {
            try
            {
                var filesNames = System.IO.Directory.GetFiles(directory.Path);
                var directoriesNames = System.IO.Directory.GetDirectories(directory.Path);
                foreach (var directoryName in directoriesNames)
                {
                    while (directories.Count >= 100)
                    {
                        Monitor.Wait(((ICollection) directories).SyncRoot);
                    }

                    var subDirectory = new Directory(directoryName, directory.Path, directory.Range + 1);
                    directories.Enqueue(subDirectory);
                    Monitor.Pulse(((ICollection) directories).SyncRoot);
                    FillDirectoriesList(subDirectory);
                }

                foreach (var fileName in filesNames)
                {
                    while (directories.Count >= 100)
                    {
                        Monitor.Wait(((ICollection) directories).SyncRoot);
                    }

                    var file = new DirectoryFile(fileName, directory.Path, directory.Range + 1);
                    directories.Enqueue(file);
                    Monitor.Pulse(((ICollection) directories).SyncRoot);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
            catch
            {
                MessageBox.Show("Some error occurred");
            }
        }
    }
}