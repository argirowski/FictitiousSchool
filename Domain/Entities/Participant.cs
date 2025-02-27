namespace Domain.Entities
{
    public class Participant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid SubmitApplicationId { get; set; }
    }
}
