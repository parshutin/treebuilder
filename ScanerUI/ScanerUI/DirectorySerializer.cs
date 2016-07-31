using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ScanerUI
{
    public class DirectorySerializer
    {
        private const string FileName = "data.xml";

        private string fullPath;

        private XmlDocument document;

        private Queue<Item> items;

        public void SetPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                fullPath = String.Format("{0}/{1}", path, FileName);
                CreateDocument();
            }
        }

        private void CreateDocument()
        {
            using (var writer = XmlWriter.Create(fullPath))
            {
                document = new XmlDocument();
                document.WriteContentTo(writer);
            }
        }

        public DirectorySerializer(Queue<Item> directories)
        {
            this.items = directories;
        }

        public void SaveToFile()
        {
            while (true)
            {
                try
                {
                    lock (((ICollection)items).SyncRoot)
                    {
                        while (items.Count == 0)
                        {
                            Monitor.Wait(((ICollection)items).SyncRoot);
                        }

                        var item = items.Peek();
                        if (item.IsInTree)
                        {
                            items.Dequeue();
                            Monitor.Pulse(((ICollection)items).SyncRoot);
                        }
                        else
                        {
                            if (item.IsInFile)
                            {
                                continue;
                            }

                            item.IsInFile = true;
                        }

                        if (item.Range == 0)
                        {
                            CreateRootNode(item);
                        }
                        else
                        {
                            var parentNode = FindParent(item, document.FirstChild);
                            if (parentNode != null)
                            {
                                AddNode(parentNode, item);
                            }
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show(e.Message);
                }
                catch (NullReferenceException e)
                {
                    MessageBox.Show(e.Message);
                }
                catch
                {
                    MessageBox.Show("Some error occurred");
                }
            }
        }

        private int ParseInt(XmlAttribute attr)
        {
            return int.Parse(attr.Value);
        }

        private XmlNode FindParent(Item item, XmlNode root)
        {
            if (ParseInt(root.Attributes["Range"]) == item.Range - 1 && root.Attributes["Path"].Value == item.ParentName)
            {
                return root;
            }

            foreach (XmlNode node in root.ChildNodes)
            {
                if (ParseInt(node.Attributes["Range"]) == item.Range - 1)
                {
                    if (node.Attributes["Path"].Value == item.ParentName)
                    {
                        return node;
                    }
                }
                else
                {
                    var parent = FindParent(item, node);
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }

            return null;
        }

        private void CreateRootNode(Item item)
        {
            var node = CreateNode(item, "Root");
            document.AppendChild(node);
            SaveDocument();
        }

        private void AddNode(XmlNode parentNode, Item item)
        {
            var node = CreateNode(item, item is DirectoryFile ? "File" : "Directory");
            parentNode.AppendChild(node);
            SaveDocument();
        }

        private void SaveDocument()
        {
            document.Save(fullPath);
        }

        private XmlNode CreateNode(Item item, string name)
        {
            var node = document.CreateNode(XmlNodeType.Element, name, null);

            var pathAttribute = document.CreateAttribute("Path");
            pathAttribute.Value = item.Path;
            node.Attributes.Append(pathAttribute);

            var rangeAttribute = document.CreateAttribute("Range");
            rangeAttribute.Value = item.Range.ToString();
            node.Attributes.Append(rangeAttribute);

            var nameAttribute = document.CreateAttribute("Name");
            nameAttribute.Value = item.Name;
            node.Attributes.Append(nameAttribute);

            var creationDateAttribute = document.CreateAttribute("CreationTimeUtc");
            creationDateAttribute.Value = item.CreationTimeUtc.ToString();
            node.Attributes.Append(creationDateAttribute);

            var lastWriteTimeAttribute = document.CreateAttribute("LastWriteTimeUtc");
            lastWriteTimeAttribute.Value = item.LastWriteTimeUtc.ToString();
            node.Attributes.Append(lastWriteTimeAttribute);

            var lastAccesTimeAttribute = document.CreateAttribute("LastAccessTimeUtc");
            lastAccesTimeAttribute.Value = item.LastAccessTimeUtc.ToString();
            node.Attributes.Append(lastAccesTimeAttribute);

            var attributesAttribute = document.CreateAttribute("Attributes");
            attributesAttribute.Value = item.Attributes;
            node.Attributes.Append(attributesAttribute);

            var accesaRulesAttribute = document.CreateAttribute("AccessRules");
            accesaRulesAttribute.Value = item.AccessRules;
            node.Attributes.Append(accesaRulesAttribute);

            var ownerAttribute = document.CreateAttribute("Owner");
            ownerAttribute.Value = item.Owner;
            node.Attributes.Append(ownerAttribute);

            var file = item as DirectoryFile;
            if (file != null)
            {
                var lengthAttribute = document.CreateAttribute("Length");
                lengthAttribute.Value = file.Length.ToString();
                node.Attributes.Append(lengthAttribute);
            }

            return node;
        }
    }
}