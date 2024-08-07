using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class Tbbill
{
    public int BillId { get; set; }

    /// <summary>
    /// نوع القائمة شراء-بيع-ارجاع شراء-ارجاع بيع
    /// </summary>
    public string? BillType { get; set; }

    public DateTime BillDate { get; set; }

    public int? BillCustomerId { get; set; }

    public decimal? BillDiscount { get; set; }

    public int? BillPaidtype { get; set; }

    public int? BilEmployeeId { get; set; }

    public string? BillNote { get; set; }

    public DateTime? BillEditDate { get; set; }

    public int? BillBoxId { get; set; }

    public string? BillMarketNumber { get; set; }

    public virtual TbEmployee? BilEmployee { get; set; }

    public virtual TbBox? BillBox { get; set; }

    public virtual TbCustomer? BillCustomer { get; set; }

    public virtual ICollection<Tbbilldetail> Tbbilldetails { get; set; } = new List<Tbbilldetail>();
}
