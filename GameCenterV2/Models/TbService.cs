using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbService : ObservableObject
{
    public int SeId { get; set; }

    public int SeIId { get; set; }
    [ObservableProperty]
    public int? seQty;

    [ObservableProperty]
    public double? sePrice;

    public double? SeDiscount { get; set; }

    /// <summary>
    /// بداية وقت اللعب
    /// </summary>
    public TimeOnly? SeStart { get; set; }

    /// <summary>
    /// عند اضافة مادة نفس الوقت بدء ونهاية
    /// </summary>
    public TimeOnly? SeEnd { get; set; }

    public int? SeTNumber { get; set; }

    public bool SeIsdel { get; set; }

    public int SeMenuId { get; set; }

    public bool SePrint { get; set; }

    public string? SeMargeTo { get; set; }

    public virtual TbItem? SeI { get; set; } = null;

    public virtual TbMenu? SeMenu { get; set; } = null;
}
