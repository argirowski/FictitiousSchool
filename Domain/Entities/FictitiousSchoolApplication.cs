namespace Domain.Entities
{
    public class FictitiousSchoolApplication
    {
        public Guid Id { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public Guid CourseDateId { get; set; }
        public CourseDate? CourseDate { get; set; }
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
