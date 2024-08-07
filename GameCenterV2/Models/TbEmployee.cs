using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbEmployee
{
    public int EmpId { get; set; }

    public string EmpName { get; set; } = null!;

    public DateOnly? EmpDatebegin { get; set; }

    public string? EmpAddress { get; set; }

    public string? EmpDepartment { get; set; }

    public string? EmpPhone { get; set; }

    public int? EmpUserId { get; set; }

    public virtual TbUser? EmpUser { get; set; }

    public virtual ICollection<TbMenu> TbMenus { get; set; } = new List<TbMenu>();

    public virtual ICollection<Tbbill> Tbbills { get; set; } = new List<Tbbill>();
}
