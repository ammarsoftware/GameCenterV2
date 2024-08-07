using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbClassify
{
    public int ClId { get; set; }

    public string ClName { get; set; } = null!;

    public string? ClDetails { get; set; }

    public virtual ICollection<Tbtransaction> Tbtransactions { get; set; } = new List<Tbtransaction>();
}
