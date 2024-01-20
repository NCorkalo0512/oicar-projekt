namespace FriendFinderAPI.Model.DTO
{
    public class MessageDTO
    {
        public int Idmessage { get; set; }
        public string? Text { get; set; }
        public string? UserSenderFullName { get; set; }
        public string? UserReceiverFullName { get; set; }
        public DateTime? DateTime { get; set; }
        public int? UserSenderId { get; set; }
        public int? UserReceiverId { get; set; }
        public bool IsSender { get; set; }
        public bool  IsDeleted { get; set; }
    }
}
