using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbTable
{
    public int TId { get; set; }

    public string TName { get; set; } = null!;

    public int TNumber { get; set; }

    public string? TDetails { get; set; }

    public string? TLocation { get; set; }

    public int? TMap { get; set; }

    public byte[]? TbImage { get; set; }

    public int? TDefaultItem { get; set; }

    public virtual TbItem? TDefaultItemNavigation { get; set; }

}
