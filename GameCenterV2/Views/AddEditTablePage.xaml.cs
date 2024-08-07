using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

public partial class AddEditTablePage : ContentPage
{
    
    public AddEditTablePage(AddTableViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
    
}