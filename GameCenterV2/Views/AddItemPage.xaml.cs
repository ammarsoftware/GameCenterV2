using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

[QueryProperty(nameof(ViewModel), "ViewModel")]
public partial class AddItemPage : ContentPage
{
	public AddItemPage(AddItemViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    private AddItemViewModel _viewModel;
    public AddItemViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            BindingContext = _viewModel;
        }
    }

}