using Dapper.Contrib.Extensions;
using System;


namespace MyAPI.Modules
{
    [Serializable]
    [Table("CoreProfiler")]
    public class CoreProfilerModulecs
    {
        [Key]
        [Write(false)]
        public long Serial { get; set; }
        public string SessionId { get; set; }
        public string Machine { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Start { get; set; }
        public int Duration { get; set; }
        public int Sort { get; set; }
        public DateTime Started { get; set; }
        public string Exception { get; set; }
    }
}
