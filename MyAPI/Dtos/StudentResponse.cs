using System;

namespace MyAPI.Dtos
{
    public sealed class StudentResponse
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
