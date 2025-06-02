namespace Domain.Entities
{
    public class Participant
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public Guid SubmitApplicationId { get; set; }
    }
}
