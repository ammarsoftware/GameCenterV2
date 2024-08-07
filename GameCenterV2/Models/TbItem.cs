using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbItem
{
    public int IId { get; set; }

    public string IName { get; set; } = null!;

    public string? IBarcode { get; set; }

    public int? IQty { get; set; }

    public float? IPriceSale { get; set; }

    public float? IPriceBuy { get; set; }

    public float? IPriceAvg { get; set; }

    public int? ILimit { get; set; }

    public string? IDetails { get; set; }

    public int? IStoreId { get; set; }

    public byte[]? IImg { get; set; }

    public bool IIstime { get; set; }

    /// <summary>
    /// طباعة للمطبخ
    /// </summary>
    public bool IPrint { get; set; }

    public int? IPrinterId { get; set; }

    /// <summary>
    /// تسلسل المادة
    /// </summary>
    public int? IOrder { get; set; }

    public virtual TbPrinter? IPrinter { get; set; }

    public virtual TbStore? IStore { get; set; }

    public virtual ICollection<TbService> TbServices { get; set; } = new List<TbService>();

    public virtual ICollection<TbTable> TbTables { get; set; } = new List<TbTable>();

    public virtual ICollection<Tbbilldetail> Tbbilldetails { get; set; } = new List<Tbbilldetail>();
}
