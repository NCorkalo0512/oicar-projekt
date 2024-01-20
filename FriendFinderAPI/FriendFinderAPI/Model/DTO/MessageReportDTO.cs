namespace FriendFinderAPI.Model.DTO
{
    public class MessageReportDTO
    {
        public int IdmessageReport { get; set; }

        public string? Reason { get; set; }

        public DateTime? DateTime { get; set; }

        public int? MessageId { get; set; }

        public int? UserSenderId { get; set; }

        public string UserSenderFullName { get; set; }

    }
}
