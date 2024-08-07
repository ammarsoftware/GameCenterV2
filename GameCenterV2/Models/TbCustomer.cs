using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbCustomer
{
    public int CuId { get; set; }

    public string CuName { get; set; } = null!;

    public string? CuPhone { get; set; }

    public string? CuAddress { get; set; }

    public string? CuPhone2 { get; set; }

    public string? CuNote { get; set; }

    public string? CuLocation { get; set; }

    public virtual ICollection<TbMenu> TbMenus { get; set; } = new List<TbMenu>();

    public virtual ICollection<Tbbill> Tbbills { get; set; } = new List<Tbbill>();

    public virtual ICollection<Tbtransaction> Tbtransactions { get; set; } = new List<Tbtransaction>();
}
