using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbUser
{
    public int UId { get; set; }

    public string? UUsername { get; set; }

    public string? UPassword { get; set; }

    public bool? UActive { get; set; }

    /// <summary>
    /// هو نفسه ro_id
    /// </summary>
    public int? UPrivilage { get; set; }

    public string? URoot { get; set; }

    public virtual ICollection<TbBox> TbBoxes { get; set; } = new List<TbBox>();

    public virtual ICollection<TbEmployee> TbEmployees { get; set; } = new List<TbEmployee>();

    public virtual ICollection<TbMenu> TbMenus { get; set; } = new List<TbMenu>();

    public virtual ICollection<Tbtransaction> Tbtransactions { get; set; } = new List<Tbtransaction>();

    public virtual TbRole? UPrivilageNavigation { get; set; }
}
