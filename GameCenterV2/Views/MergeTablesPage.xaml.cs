using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;
[QueryProperty(nameof(ViewModel), "ViewModel")]
public partial class MergeTablesPage : ContentPage
{
	private MergeTablesViewModel _viewModel;
	public MergeTablesPage(MergeTablesViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
    public MergeTablesViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            BindingContext = _viewModel;
        }
    }

}