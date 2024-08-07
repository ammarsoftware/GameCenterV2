using GameCenterV2.Models;
using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

public partial class ReceiptPage : ContentPage
{
	public ReceiptPage(List<TbService> orderItems)
	{
		InitializeComponent();
        BindingContext = new ReceiptViewModel(orderItems);
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        //عند استعمال هذا السطر يتوقف التطبيق
        //await Shell.Current.Navigation.PopAsync();
    }
}