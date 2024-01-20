using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class UserType
{
    public int IduserType { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
