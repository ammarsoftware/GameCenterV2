using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

[QueryProperty(nameof(ViewModel), "ViewModel")]
public partial class AddStorePage : ContentPage
{
	public AddStorePage(AddStoreViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private AddStoreViewModel _viewModel;
    public AddStoreViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            BindingContext = _viewModel;
        }
    }
}