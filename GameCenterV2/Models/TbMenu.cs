using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbMenu
{
    public int MId { get; set; }

    public DateTime MDate { get; set; }

    public double? MDiscount { get; set; }

    public bool MIsactive { get; set; }

    public int? MUId { get; set; }

    public int? MBoxId { get; set; }

    public string? MNote { get; set; }

    /// <summary>
    /// الغاء الوصل
    /// </summary>
    public bool? MCancel { get; set; }

    /// <summary>
    /// داخل-سفري-دلفري
    /// </summary>
    public string? MType { get; set; }

    /// <summary>
    /// في حال كانت القائمة دلفري
    /// </summary>
    public int? MCuId { get; set; }

    /// <summary>
    /// هة نفسه الموظفين
    /// </summary>
    public int? MDriverId { get; set; }

    public decimal? MDeleveryPrice { get; set; }

    public virtual TbBox? MBox { get; set; }

    public virtual TbCustomer? MCu { get; set; }

    public virtual TbEmployee? MDriver { get; set; }

    public virtual TbUser? MU { get; set; }

    public virtual ICollection<TbService> TbServices { get; set; } = new List<TbService>();
}
