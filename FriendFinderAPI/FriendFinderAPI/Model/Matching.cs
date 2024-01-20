using System;
using System.Collections.Generic;

namespace FriendFinderAPI.Model;

public partial class Matching
{
    public int Idmatching { get; set; }

    public bool? Swipe { get; set; }

    public int? UserId1 { get; set; }

    public int? UserId2 { get; set; }

    public DateTime? DateTime { get; set; }

    public virtual User? UserId1Navigation { get; set; }

    public virtual User? UserId2Navigation { get; set; }
}
