using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

/// <summary>
/// امين الصندوق اليومي
/// </summary>
public partial class TbBox
{
    public int BoxId { get; set; }

    public DateTime? BoxDateIn { get; set; }

    public DateTime? BoxDateOut { get; set; }

    public double? BoxMonyIn { get; set; }

    public double? BoxMonyOut { get; set; }

    public string? BoxDetails { get; set; }

    /// <summary>
    /// اليوزر الذي فتح الصندوق
    /// </summary>
    public int? BoxUId { get; set; }

    public bool BoxIsopen { get; set; }

    public virtual TbUser? BoxU { get; set; }

    public virtual ICollection<TbMenu> TbMenus { get; set; } = new List<TbMenu>();

    public virtual ICollection<Tbbill> Tbbills { get; set; } = new List<Tbbill>();

    public virtual ICollection<Tbtransaction> Tbtransactions { get; set; } = new List<Tbtransaction>();
}
