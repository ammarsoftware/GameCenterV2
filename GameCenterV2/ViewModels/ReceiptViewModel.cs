using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace GameCenterV2.ViewModels
{
    
    public partial class ReceiptViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<TbService> orderItems;

        [ObservableProperty]
        private double total;

        public ReceiptViewModel(List<TbService> items)
        {
            OrderItems = new ObservableCollection<TbService>(items);
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            Total = OrderItems.Sum(item => (item.SePrice ?? 0) * (item.SeQty ?? 0));
        }

        [RelayCommand]
        private async Task PrintReceipt()
        {
            try
            {
                await ReceiptPrintService.PrintReceipt(OrderItems.ToList(), Total);
                await Application.Current.MainPage.DisplayAlert("نجاح", "تمت طباعة الفاتورة بنجاح", "موافق");
                await Shell.Current.GoToAsync("//MapPage/PosPage");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("خطأ", $"حدث خطأ أثناء الطباعة: {ex.Message}", "موافق");
            }
        }
    }
}
