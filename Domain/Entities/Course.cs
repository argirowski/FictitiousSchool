﻿namespace Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseDate> CourseDates { get; set; }
    }
}
