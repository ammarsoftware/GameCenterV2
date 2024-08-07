using GameCenterV2.ViewModels;
using Mopups.Pages;

namespace GameCenterV2.SubView
{
    public partial class PricePopup : PopupPage
    {
        public PricePopup(string initialPrice)
        {
            InitializeComponent();
            BindingContext = new PricePopupViewModel(initialPrice);
        }
    }
}