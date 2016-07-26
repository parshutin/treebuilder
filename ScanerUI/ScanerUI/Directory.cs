namespace ScanerUI
{
    public class Directory : IItem
    {
        public Directory()
        {
            
        }

        public Directory(string path, string parentName, int range)
        {
            Path = path;
            ParentName = parentName;
            Range = range;
        }

        public string Path { get;  set; }

        public string ParentName { get;  set; }

        public int Range { get;  set; }
    }
}