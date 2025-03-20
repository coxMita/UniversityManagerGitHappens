using System.Collections.Generic;

namespace UniversityManager.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public List<int> Subjects { get; set; } = new();
    }
}
