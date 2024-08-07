using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbRole
{
    public int RoId { get; set; }

    public string RoName { get; set; } = null!;

    public string RoDetiles { get; set; } = null!;

    public virtual ICollection<TbPrivilege> TbPrivileges { get; set; } = new List<TbPrivilege>();

    public virtual ICollection<TbUser> TbUsers { get; set; } = new List<TbUser>();
}
