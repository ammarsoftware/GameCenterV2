using System;
using System.Collections.Generic;

namespace GameCenterV2.Models;

public partial class TbCompany
{
    public int CId { get; set; }

    public string CName { get; set; } = null!;

    public string? CAddress { get; set; }

    public string? CPhone { get; set; }

    public string? CPhone1 { get; set; }

    public string? CEmail { get; set; }

    public byte[]? CSnap { get; set; }

    public string? CInsta { get; set; }

    public byte[]? CLogo { get; set; }
}
