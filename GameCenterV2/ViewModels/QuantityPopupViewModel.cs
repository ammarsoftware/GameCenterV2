using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace GameCenterV2.ViewModels
{
    public partial class QuantityPopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string quantity;
       
        public QuantityPopupViewModel(string initialQuantity)
        {
            Quantity = initialQuantity;
        }
        [RelayCommand]
        private void AddNumber(string number)
        {
            Quantity += number;
        }
        [RelayCommand]
        private void Delete()
        {
            if (Quantity.Length > 0)
            {
                Quantity = Quantity.Substring(0, Quantity.Length - 1);
            }
        }
        [RelayCommand]
        private async void Confirm()
        {
            // إغلاق النافذة المنبثقة وإرجاع القيمة
            await MopupService.Instance.PopAsync();
            MessagingCenter.Send(this, "QuantityUpdated", Quantity);
        }
    }
}