namespace Services.Events
{
    public struct DeleteFileEvent
    {
        public string Path { get; set; }

        public DeleteFileEvent(string path)
        {
            Path = path;
        }
    }

    public struct FileDeletedEvent
    {
        public string Path { get; set; }
        public bool Success { get; set; }

        public FileDeletedEvent(string path, bool success)
        {
            Path = path;
            Success = success;
        }
    }

    public struct DeleteDirectoryEvent
    {
        public string Path { get; set; }

        public DeleteDirectoryEvent(string path)
        {
            Path = path;
        }
    }

    public struct DirectoryDeletedEvent
    {
        public string Path { get; set; }
        public bool Success { get; set; }

        public DirectoryDeletedEvent(string path, bool success)
        {
            Path = path;
            Success = success;
        }
    }
}