using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScanerUI
{
    public class SyncronizationEvents
    {
        public SyncronizationEvents()
        {
            FolderScanned = new AutoResetEvent(false);
            FolderSerialized = new AutoResetEvent(false);
            ThreadExited = new ManualResetEvent(false);
            eventArray = new WaitHandle[3];
            eventArray[0] = FolderScanned;
            eventArray[1] = FolderSerialized;
            eventArray[2] = ThreadExited;
        }

        public EventWaitHandle FolderScanned { get; private set; }

        public EventWaitHandle FolderSerialized { get; private set; }

        public EventWaitHandle ThreadExited { get; private set; }

        public WaitHandle[] eventArray { get; private set; }
    }
}
