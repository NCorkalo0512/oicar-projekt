using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class Message
{
    public int Idmessage { get; set; }

    public string? Text { get; set; }

    public DateTime? DateTime { get; set; }

    public DateTime? SeenDateTime { get; set; }

    public int? UserSenderId { get; set; }

    public int? UserReceiverId { get; set; }

    public virtual ICollection<DeletedMessage> DeletedMessages { get; } = new List<DeletedMessage>();

    public virtual ICollection<MessageReport> MessageReports { get; } = new List<MessageReport>();

    public virtual User? UserReceiver { get; set; }

    public virtual User? UserSender { get; set; }
}
