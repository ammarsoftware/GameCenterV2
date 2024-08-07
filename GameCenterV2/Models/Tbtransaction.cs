using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class Tbtransaction
{
    public int TransactionId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal TransactionAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? TransactionDescription { get; set; }

    public int? TransactionClassifyId { get; set; }

    public int? TransactionUserId { get; set; }

    public int? TransactionBoxId { get; set; }

    public int? TransactionCustomerId { get; set; }

    public virtual TbBox? TransactionBox { get; set; }

    public virtual TbClassify? TransactionClassify { get; set; }

    public virtual TbCustomer? TransactionCustomer { get; set; }

    public virtual TbUser? TransactionUser { get; set; }
}
