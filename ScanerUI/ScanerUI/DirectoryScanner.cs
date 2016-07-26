using System;
using System.Collections;
using System.Collections.Generic;

namespace ScanerUI
{
    public class DirectoryScanner
    {
        private Directory directory;

        private SyncronizationEvents events;

        public event EventHandler ScanCompleted;

        private Queue<IItem> directories;

        private string path;

        public DirectoryScanner(string path, Queue<IItem> directories, SyncronizationEvents events)
        {
            this.path = path;
            this.directories = directories;
            this.events = events;
        }

        public DirectoryScanner(Directory directory, SyncronizationEvents events)
        {
            this.directory = directory;
            this.events = events;
        }

        public void StartScan()
        {
            lock (((ICollection)directories).SyncRoot)
            {
                var root = new Directory(path, null, 0);
                directories.Enqueue(root);
                events.FolderScanned.Set();
                FillDirectoriesList(root);
            }

            if (ScanCompleted != null)
            {
                ScanCompleted(this, EventArgs.Empty);
            }
        }

        private void FillDirectoriesList(Directory directory)
        {
            var filesNames = System.IO.Directory.GetFiles(directory.Path);
            var directoriesNames = System.IO.Directory.GetDirectories(directory.Path);
            foreach (var directoryName in directoriesNames)
            {
                var subDirectory = new Directory(directoryName, directory.Path, directory.Range + 1);
                directories.Enqueue(subDirectory);
                events.FolderScanned.Set();
                FillDirectoriesList(subDirectory);
            }

            foreach (var fileName in filesNames)
            {
                var file = new DirectoryFile(fileName, directory.Path, directory.Range + 1);
                directories.Enqueue(file);
                events.FolderScanned.Set();
            }
        }

        /*public void StartScan()
        {
            FillDirectoriesList(directory);
            if (ScanCompleted != null)
            {
                ScanCompleted(this, EventArgs.Empty);
            }
        }

        private void FillDirectoriesList(Directory directory)
        {
            lock (directory.Locker)
            {
                var filesNames = System.IO.Directory.GetFiles(directory.Path);
                var directoriesNames = System.IO.Directory.GetDirectories(directory.Path);
                foreach (var directoryName in directoriesNames)
                {
                    var subDirectory = new Directory(directoryName);
                    directory.Directories.Add(subDirectory);
                    Console.WriteLine("Added Directory");
                    events.FolderScanned.Set();
                    FillDirectoriesList(subDirectory);
                }


                foreach (var fileName in filesNames)
                {
                    directory.DirectoryFiles.Add(new DirectoryFile(fileName, directory));
                    Console.WriteLine("Added File");
                    events.FolderScanned.Set();
                }
            }
        }*/
    }
}