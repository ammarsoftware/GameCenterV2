using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbPrivilege
{
    public int Id { get; set; }

    public string FormName { get; set; } = null!;

    public bool CanUpdate { get; set; }

    public bool CanRead { get; set; }

    public bool CanCreate { get; set; }

    public bool CanDelete { get; set; }

    public int RoleId { get; set; }

    public virtual TbRole Role { get; set; } = null!;
}
