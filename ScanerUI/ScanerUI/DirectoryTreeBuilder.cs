using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanerUI
{
    public class DirectoryTreeBuilder
    {
        private TreeView treeView;

        private Queue<Item> items;

        public DirectoryTreeBuilder(Queue<Item> directories, TreeView treeView)
        {
            this.items = directories;
            this.treeView = treeView;
        }

        public void BuildDirectoryTree()
        {
            while (true)
            {
                try
                {
                    lock (((ICollection) items).SyncRoot)
                    {
                        while (items.Count == 0)
                        {
                            Monitor.Wait(((ICollection) items).SyncRoot);
                        }


                        var item = items.Peek();
                        if (item.IsInFile)
                        {
                            items.Dequeue();
                            Monitor.Pulse(((ICollection) items).SyncRoot);
                        }
                        else
                        {
                            if (item.IsInTree)
                            {
                                continue;
                            }

                            item.IsInTree = true;
                        }

                        var node = new TreeNode(item.Path);
                        if (item.Range == 0)
                        {
                            AddNode(node);
                        }
                        else
                        {
                            var parentNode = FindParent(item, treeView.Nodes[0]);
                            if (parentNode != null)
                            {
                                AddNode(parentNode, node);
                            }
                        }
                    }
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

        private TreeNode FindParent(Item item, TreeNode root)
        {
            if (root.Level == item.Range - 1 && root.Text == item.ParentName)
            {
                return root;
            }

            foreach (TreeNode node in root.Nodes)
            {
                if (node.Level == item.Range - 1)
                {
                    if (node.Text == item.ParentName)
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

        private void AddNode(TreeNode node)
        {
            if (treeView.InvokeRequired)
            {
                treeView.Invoke(new AddTreeNode(AddNode), node);
            }
            else
            {
                treeView.Nodes.Add(node);
            }
        }

        private void AddNode(TreeNode parent, TreeNode child)
        {
            if (treeView.InvokeRequired)
            {
                treeView.Invoke(new AddTreeNodeToNode(AddNode), new object[] { parent, child });
            }
            else
            {
                parent.Nodes.Add(child);
            }
        }

        private delegate void AddTreeNode(TreeNode node);

        private delegate void AddTreeNodeToNode(TreeNode parent, TreeNode child);
    }
}