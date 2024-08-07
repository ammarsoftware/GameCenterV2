using GameCenterV2.ViewModels;
using Mopups.Pages;

namespace GameCenterV2.SubView;

public partial class QuantityPopup : PopupPage
{
    public QuantityPopup(string initialQuantity)
    {
        InitializeComponent();
        BindingContext = new QuantityPopupViewModel(initialQuantity);
    }
}