using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class User
{
    public int Iduser { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public byte[] PasswordHash { get; set; }

    public DateTime? CreateDateTime { get; set; }

    public bool? Verification { get; set; }

    public int? UserTypeId { get; set; }

    public virtual ICollection<Matching> MatchingUserId1Navigations { get; } = new List<Matching>();

    public virtual ICollection<Matching> MatchingUserId2Navigations { get; } = new List<Matching>();

    public virtual ICollection<MessageReport> MessageReports { get; } = new List<MessageReport>();

    public virtual ICollection<Message> MessageUserReceivers { get; } = new List<Message>();

    public virtual ICollection<Message> MessageUserSenders { get; } = new List<Message>();

    public virtual ICollection<Notification> NotificationUserRecivers { get; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationUserSenders { get; } = new List<Notification>();

    public virtual ICollection<Profile> Profiles { get; } = new List<Profile>();

    public virtual UserType? UserType { get; set; }
}
