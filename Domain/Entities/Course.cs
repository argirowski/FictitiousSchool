namespace Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required ICollection<CourseDate> CourseDates { get; set; }
    }
}
