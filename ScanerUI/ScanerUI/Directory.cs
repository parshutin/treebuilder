using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;

namespace ScanerUI
{
    public class Directory : Item
    {
        public Directory(string path, string parentName, int range)
        {
            try
            {
                Path = path;
                ParentName = parentName;
                Range = range;
                var directoryInfo = new FileInfo(path);
                Name = directoryInfo.Name;
                CreationTimeUtc = directoryInfo.CreationTimeUtc;
                LastWriteTimeUtc = directoryInfo.LastWriteTimeUtc;
                LastAccessTimeUtc = directoryInfo.LastAccessTimeUtc;
                ParseAttributes(directoryInfo.Attributes);
                GetSystemRights(directoryInfo);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /*private void GetSystemRights(FileInfo directoryInfo)
        {
            var rules = new HashSet<string>();
            var accessControl = directoryInfo.GetAccessControl();
            Owner = accessControl.GetOwner(typeof (NTAccount)).ToString();
            var accesRules = accessControl.GetAccessRules(true, true, typeof (NTAccount));
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(user);
            foreach (FileSystemAccessRule rule in accesRules)
            {
                if (principal.IsInRole(rule.IdentityReference.Value))
                {
                    if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                    {
                        rules.Add(FileSystemRights.Read.ToString());
                    }

                    if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                    {
                        rules.Add(FileSystemRights.Write.ToString());
                    }

                    if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete)
                    {
                        rules.Add(FileSystemRights.Delete.ToString());
                    }

                    if ((FileSystemRights.CreateFiles & rule.FileSystemRights) == FileSystemRights.CreateFiles)
                    {
                        rules.Add(FileSystemRights.CreateFiles.ToString());
                    }

                    if ((FileSystemRights.ExecuteFile & rule.FileSystemRights) == FileSystemRights.ExecuteFile)
                    {
                        rules.Add(FileSystemRights.ExecuteFile.ToString());
                    }
                }
            }

            AccessRules = string.Join(",", rules);
        }

        private void ParseAttributes(FileAttributes attributes)
        {
            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                Attributes += FileAttributes.Hidden + ",";
            }

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                Attributes += FileAttributes.ReadOnly + ",";
            }

            if ((attributes & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed)
            {
                Attributes += FileAttributes.NotContentIndexed + ",";
            }

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Attributes += FileAttributes.Directory + ",";
            }

            if ((attributes & FileAttributes.System) == FileAttributes.System)
            {
                Attributes += FileAttributes.System + ",";
            }

            Attributes = Attributes.Substring(0, Attributes.Length - 1);
        }

        public string Path { get; set; }

        public string ParentName { get; set; }

        public int Range { get; set; }

        public bool IsInTree { get; set; }

        public bool IsInFile { get; set; }

        public string Name { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }

        public DateTime LastAccessTimeUtc { get; set; }

        public string Attributes { get; set; }

        public string AccessRules { get; set; }

        public string Owner { get; set; }*/
    }
}