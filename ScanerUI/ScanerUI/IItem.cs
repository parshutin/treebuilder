using System;
using System.Collections.Generic;
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
    }
}
