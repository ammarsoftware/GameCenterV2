using GameCenterV2.Models;
using GameCenterV2.Services;
using GameCenterV2.ViewModels;
using iText.Layout.Properties;
using System.Diagnostics;

namespace GameCenterV2.Views;

[QueryProperty(nameof(ViewModel), "ViewModel")]
public partial class PosPage : ContentPage
{
    private PosViewModel _viewModel;

    public PosPage(PosViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    public PosViewModel ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            BindingContext = _viewModel;
        }
    }

    //لتمرير البيانات
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TbStore selectedCategory)
        {
            _viewModel.LoadFoodItemsCommand.Execute(selectedCategory.StoreName);
        }
    }

    private void OnFoodItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TbItem selectedItem)
        {
            _viewModel.AddToOrderCommand.Execute(selectedItem);
        }
    }
}