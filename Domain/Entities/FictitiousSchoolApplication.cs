namespace Domain.Entities
{
    public class FictitiousSchoolApplication
    {
        public Guid Id { get; set; }
        public int CourseId { get; set; }
        public required Course Course { get; set; }
        public Guid CourseDateId { get; set; }
        public required CourseDate CourseDate { get; set; }
        public Guid CompanyId { get; set; }
        public required Company Company { get; set; }
        public required ICollection<Participant> Participants { get; set; }
    }
}
