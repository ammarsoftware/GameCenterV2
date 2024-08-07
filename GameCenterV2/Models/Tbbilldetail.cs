using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class Tbbilldetail
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public decimal Price { get; set; }

    public int BillId { get; set; }

    public int Quantity { get; set; }

    public virtual Tbbill Bill { get; set; } = null!;

    public virtual TbItem Item { get; set; } = null!;
}
