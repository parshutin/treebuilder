﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace ScanerUI
{
    public class DirectoryFile : Item
    {
        public DirectoryFile()
        {
        }

        public DirectoryFile(string path, string parentName, int range)
        {
            try
            {
                Path = path;
                ParentName = parentName;
                Range = range;
                var fileInfo = new FileInfo(path);
                Name = fileInfo.Name;
                CreationTimeUtc = fileInfo.CreationTimeUtc;
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
                LastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
                Length = fileInfo.Length;
                ParseAttributes(fileInfo.Attributes);
                GetSystemRights(fileInfo);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

       /* private void GetSystemRights(FileInfo fileInfo)
        {
            var rules = new HashSet<string>();
            var accessControl = fileInfo.GetAccessControl();
            Owner = accessControl.GetOwner(typeof (NTAccount)).ToString();
            var accesRules = accessControl.GetAccessRules(true, true, typeof(NTAccount));
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

            if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                Attributes += FileAttributes.Archive + ",";
            }

            if ((attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted)
            {
                Attributes += FileAttributes.Encrypted + ",";
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

        public long Length { get; set; }
    }
}