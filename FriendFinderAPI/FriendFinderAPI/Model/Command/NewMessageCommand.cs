namespace FriendFinderAPI.Model.Command
{
    public class NewMessageCommand
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; }
    }
}
