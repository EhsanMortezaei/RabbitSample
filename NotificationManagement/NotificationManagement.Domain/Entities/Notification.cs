namespace NotificationManagement.Domain.Entities
{
    public sealed class Notification
    {
        public int Id { get; set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string Recipient { get; private set; }

        private Notification() { }

        public Notification(string title, string body, string recipient)
        {
            Title = title;
            Body = body;
            Recipient = recipient;
        }
    }
}
