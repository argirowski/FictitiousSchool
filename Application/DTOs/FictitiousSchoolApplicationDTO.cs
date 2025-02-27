namespace Application.DTOs
{
    public class FictitiousSchoolApplicationDTO
    {
        public Guid Id { get; set; }
        public CourseDTO Course { get; set; }
        public CourseDateDTO CourseDate { get; set; }
        public CompanyDTO Company { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
    }
}
