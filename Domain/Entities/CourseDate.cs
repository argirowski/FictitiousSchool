namespace Domain.Entities
{
    public class CourseDate
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
