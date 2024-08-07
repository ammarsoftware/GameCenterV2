using GameCenterV2.Models;
using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

public partial class ItemListPage : ContentPage
{
    private ItemListViewModel _viewmodel;
	public ItemListPage(ItemListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewmodel = viewModel;

    }
    //áÊÍÏíË ÇáÈíÇäÇÊ ÈÚÏ ÇáÊÍÏíË Çæ ÇáÇÖÇİÉ
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // ÊÍÏíË ÇáÈíÇäÇÊ ÚäÏ ÙåæÑ ÇáÕİÍÉ
        await _viewmodel.GetitemAsync(); // ÊÃßÏ ãä Ãä áÏíß ÏÇáÉ áÊÍãíá ÇáÈíÇäÇÊ
    }
    private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TbItem selectedItem)
        {
            // ÅÚÇÏÉ ÊÚííä ÇáÇÎÊíÇÑ
            ((CollectionView)sender).SelectedItem = null;

            // ÇÓÊÏÚÇÁ ÇáÃãÑ İí ViewModel
            if (BindingContext is ItemListViewModel viewModel)
            {
                await viewModel.ShowOptionsForItemAsync(selectedItem);
            }
        }
    }
}