using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;
[QueryProperty(nameof(ViewModel), "ViewModel")]
public partial class OpenBoxPage : ContentPage
{
	private OpenBoxViewModel _viewModel;
	public OpenBoxPage(OpenBoxViewModel openBoxView)
	{
		InitializeComponent();
		_viewModel = openBoxView;
		BindingContext = _viewModel;
	}
    public OpenBoxViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            BindingContext = _viewModel;
        }
    }
}