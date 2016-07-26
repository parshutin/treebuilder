namespace ScanerUI
{
    public class DirectoryFile : IItem
    {
        public DirectoryFile()
        {
        }

        public DirectoryFile(string path, string parentName, int range)
        {
            Path = path;
            ParentName = parentName;
            Range = range;
        }

        public string Path { get; set; }

        public string ParentName { get; set; }

        public int Range { get; set; }
    }
}