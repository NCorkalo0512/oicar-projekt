using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class Notification
{
    public int Idnotifications { get; set; }

    public int? UserReciverId { get; set; }

    public int? UserSenderId { get; set; }

    public string? Content { get; set; }

    public DateTime? DateTime { get; set; }

    public DateTime? SeenDateTime { get; set; }

    public virtual User? UserReciver { get; set; }

    public virtual User? UserSender { get; set; }
}
