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
        private SyncronizationEvents syncEvents;

        private TreeView treeView;

        private Queue<IItem> items;

        public DirectoryTreeBuilder(Queue<IItem> directories, SyncronizationEvents events, TreeView treeView)
        {
            this.items = directories;
            this.syncEvents = events;
            this.treeView = treeView;
        }

        public void BuildDirectoryTree()
        {
            while (WaitHandle.WaitAny(syncEvents.eventArray) != 1)
            {
                lock (((ICollection)items).SyncRoot)
                {
                    while (items.Count > 0)
                    {
                        var item = items.Dequeue();
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
            }
        }

        private TreeNode FindParent(IItem item, TreeNode root)
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