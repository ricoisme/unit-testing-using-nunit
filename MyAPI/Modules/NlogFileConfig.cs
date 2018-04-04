namespace MyAPI.Infarstructure
{
    public class NlogFileConfig
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Layout { get; set; }
        public string LogLevel { get; set; }
        public string ArchiveFileName { get; set; }
        public string ArchiveNumbering { get; set; }
        public string ArchiveEvery { get; set; }
        public string ArchiveDateFormat { get; set; }
    }
}
