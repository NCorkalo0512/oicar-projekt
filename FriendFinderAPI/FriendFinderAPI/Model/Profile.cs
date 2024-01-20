using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class Profile
{
    public int Idprofile { get; set; }

    public string? Technology { get; set; }

    public string? ProjectDescription { get; set; }

    public bool? Enabled { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
