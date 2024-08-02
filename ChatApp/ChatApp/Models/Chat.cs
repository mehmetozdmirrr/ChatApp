namespace ChatApp.Models
{
    public sealed class Chat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int toUserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
    }
}
