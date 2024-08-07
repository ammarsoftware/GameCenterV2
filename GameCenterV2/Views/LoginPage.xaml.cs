using GameCenterV2.Models;
using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
        
        FlowDirection = FlowDirection.RightToLeft;
        BindingContext = viewModel;
    }
}