namespace FriendFinderAPI.Model.Command
{
    public class MessageReportCommand
    {
        public string? Reason { get; set; }

        public int? MessageId { get; set; }

        public int? UserSenderId { get; set; }
    }
}
