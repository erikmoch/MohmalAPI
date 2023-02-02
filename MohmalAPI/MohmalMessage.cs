namespace MohmalAPI
{
    public class MohmalMessage
    {
        public int Id { get; }
        public string Subject { get; }
        public string Sender { get; }
        public string Content { get; }

        public MohmalMessage(string id, string subject, string sender, string content)
        {
            Id = int.Parse(id);
            Subject = subject;
            Sender = sender;
            Content = content;
        }
    }
}