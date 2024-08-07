using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCenterV2.Models
{
    public partial class TableShow : ObservableObject
    {
        [ObservableProperty] bool isActive;
        [ObservableProperty] bool isMarged;
        [ObservableProperty] int tId;
        [ObservableProperty] string? tName;
        [ObservableProperty] int tNumber;
        [ObservableProperty] int? tMap;
        [ObservableProperty] byte[]? tbImage;
        [ObservableProperty] string? tDetails;
        [ObservableProperty] string? tLocation;
        [ObservableProperty] int? tDefaultItem;
        [ObservableProperty] int? menuid;
        [ObservableProperty] bool isSelected;
        public virtual TbItem? TDefaultItemNavigation { get; set; }
    }
}
