namespace FriendFinderAPI.Model.Command
{
    public class UpdateUserPasswordCommand
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
