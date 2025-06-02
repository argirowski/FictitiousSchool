namespace Application.DTOs
{
    public class FictitiousSchoolApplicationDTO
    {
        public Guid Id { get; set; }
        public required CourseDTO Course { get; set; }
        public required CourseDateDTO CourseDate { get; set; }
        public required CompanyDTO Company { get; set; }
        public required List<ParticipantDTO> Participants { get; set; }
    }
}
