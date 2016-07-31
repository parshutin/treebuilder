using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanerUI
{
    public partial class Form1 : Form
    {
        private string folderPath;

        private string destinationFolderPath;

        private Queue<Item> items;

        private DirectoryTreeBuilder treeBuilder;

        private DirectorySerializer directorySerializer;

        private Thread scanThread;

        private Thread buildThread;

        private Thread seriazizeThread;

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = false;
            items = new Queue<Item>();
            treeBuilder = new DirectoryTreeBuilder(items, treeView1);
            directorySerializer = new DirectorySerializer(items);
            buildThread = new Thread(treeBuilder.BuildDirectoryTree);
            buildThread.IsBackground = true;
            seriazizeThread = new Thread(directorySerializer.SaveToFile);
            seriazizeThread.IsBackground = true;
            buildThread.Start();
            seriazizeThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                folderLabel.Text = folderPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Select folder first before scan");
                return;
            }

            if (string.IsNullOrEmpty(destinationFolderPath))
            {
                MessageBox.Show("Select folder for result document first before scan");
                return;
            }

            items.Clear();
            treeView1.Nodes.Clear();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            progressBar1.Visible = true;
            var directoryScanner = new DirectoryScanner(this.folderPath, items);
            directoryScanner.ScanCompleted += DirectoryScanner_ScanCompleted;
            directorySerializer.SetPath(destinationFolderPath);
            scanThread = new Thread(directoryScanner.StartScan);
            scanThread.IsBackground = true;
            scanThread.Start();
        }

        private void DirectoryScanner_ScanCompleted(object sender, EventArgs e)
        {
            ShowButtons();
            scanThread.Join();
        }

        private void ShowButtons()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    progressBar1.Visible = false;
                }));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                destinationFolderPath = folderBrowserDialog1.SelectedPath;
                destinationFolder.Text = destinationFolderPath;
            }
        }
    }
}
