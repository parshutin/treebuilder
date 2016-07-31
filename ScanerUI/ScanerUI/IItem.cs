using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanerUI
{
    public interface IItem
    {
        string Path { get; set; }

        string ParentName { get; set; }

        int Range { get; set; }

        bool IsInTree { get; set; }

        bool IsInFile { get; set; }

        string Name { get; set; }

        DateTime CreationTimeUtc { get; set; }

        DateTime LastWriteTimeUtc { get; set; }

        DateTime LastAccessTimeUtc { get; set; }

        string Attributes { get; set; }

        string AccessRules { get; set; }

        string Owner { get; set; }
    }
}
