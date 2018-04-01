using Dapper.Contrib.Extensions;
using System;

namespace MyAPI.Modules
{
    [Serializable]
    [Table("EventLog")]
    public class EventLogModule
    {
        [Key]
        [Write(false)]
        public long Id { get; set; }
        public int EventID { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        [Write(false)]
        public DateTime CreatedTime { get; set; }
    }
}
