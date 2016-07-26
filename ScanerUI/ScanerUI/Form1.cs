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

        private Directory directory;

        private SyncronizationEvents syncEvents;

        private Thread scanThread;

        public Form1()
        {
            InitializeComponent();
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = false;
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

            treeView1.Nodes.Clear();
            button1.Enabled = false;
            button2.Enabled = false;
            progressBar1.Visible = true;

            syncEvents = new SyncronizationEvents();
            var items = new Queue<IItem>();
            //var directoryScanner = new DirectoryScanner(directory, syncEvents);
            var directoryScanner = new DirectoryScanner(this.folderPath, items, syncEvents);
            directoryScanner.ScanCompleted += DirectoryScanner_ScanCompleted;
            //var directoryTreeBuilder = new DirectoryTreeBuilder(directory, syncEvents, treeView1);
            var directoryTreeBuilder = new DirectoryTreeBuilder(items, syncEvents, treeView1);
            //var directorySerializer = new DirectorySerializer(directory, syncEvents);
            scanThread = new Thread(directoryScanner.StartScan);
            var buildThread = new Thread(directoryTreeBuilder.BuildDirectoryTree);
            //var seriazizeThread = new Thread(directorySerializer.Serialize);
            Console.WriteLine("Launching producer and consumer threads...");
            scanThread.Start();
            buildThread.Start();
            //seriazizeThread.Start();

            /* for (int i = 0; i < 3; i++)
             {
                 Thread.Sleep(10);
                 ShowCount(directory);
             }*/

            
            //buildThread.Join();
            //seriazizeThread.Join();
            // BuildDirectoryTree();
        }

        private void DirectoryScanner_ScanCompleted(object sender, EventArgs e)
        {
           // var serializer = new DirectorySerializer();
           // serializer.WriteScores(directory);
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
                    progressBar1.Visible = false;
                }));
            }
        }
    }
}
