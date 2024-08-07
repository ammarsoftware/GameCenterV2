using GameCenterV2.Models;
using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

public partial class StoreListPage : ContentPage
{
    private StoreListViewModel _viewmodel;

    public StoreListPage(StoreListViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewmodel = viewModel;
    }
    //· ÕœÌÀ «·»Ì«‰«  »⁄œ «· ÕœÌÀ «Ê «·«÷«›…
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //  ÕœÌÀ «·»Ì«‰«  ⁄‰œ ŸÂÊ— «·’›Õ…
        await _viewmodel.GetStoreAsync(); //  √ﬂœ „‰ √‰ ·œÌﬂ œ«·… · Õ„Ì· «·»Ì«‰« 
    }
    private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TbStore selectedItem)
        {
            // ≈⁄«œ…  ⁄ÌÌ‰ «·«Œ Ì«—
            ((CollectionView)sender).SelectedItem = null;

            // «” œ⁄«¡ «·√„— ›Ì ViewModel
            if (BindingContext is StoreListViewModel viewModel)
            {
                await viewModel.ShowOptionsForItemAsync(selectedItem);
            }
        }
    }
}