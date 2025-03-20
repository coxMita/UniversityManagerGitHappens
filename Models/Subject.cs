using System.Collections.Generic;

namespace UniversityManager.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = "";

        public List<int> StudentsEnrolled { get; set; } = new();
    }
}
