namespace DatingApp.Domain.Aggregates.AppUser.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public AppUser Sender { get; set; } = new();

        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public AppUser Receiver { get; set; } = new();

        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }

    }
}
