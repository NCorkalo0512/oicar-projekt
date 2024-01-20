using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class MessageReport
{
    public int IdmessageReport { get; set; }

    public string? Reason { get; set; }

    public string? Explanation { get; set; }

    public DateTime? DateTime { get; set; }

    public int? MessageId { get; set; }

    public int? UserSenderId { get; set; }

    public virtual Message? Message { get; set; }

    public virtual User? UserSender { get; set; }
}
