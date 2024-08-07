using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace GameCenterV2.ViewModels
{
    public partial class PricePopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string price;
       

        public PricePopupViewModel(string initialPrice)
        {
            Price = initialPrice;

        }
        [RelayCommand]
        private void AddNumber(string number)
        {
            if (number == "." && Price.Contains("."))
                return;
            Price += number;
        }
        [RelayCommand]
        private void Delete()
        {
            if (Price.Length > 0)
            {
                Price = Price.Substring(0, Price.Length - 1);
            }
        }
        [RelayCommand]
        private async void Confirm()
        {
            await MopupService.Instance.PopAsync();
            MessagingCenter.Send(this, "PriceUpdated", Price);
        }
    }
}