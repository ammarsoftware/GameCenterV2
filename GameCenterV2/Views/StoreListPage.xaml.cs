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
    //������ �������� ��� ������� �� �������
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // ����� �������� ��� ���� ������
        await _viewmodel.GetStoreAsync(); // ���� �� �� ���� ���� ������ ��������
    }
    private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TbStore selectedItem)
        {
            // ����� ����� ��������
            ((CollectionView)sender).SelectedItem = null;

            // ������� ����� �� ViewModel
            if (BindingContext is StoreListViewModel viewModel)
            {
                await viewModel.ShowOptionsForItemAsync(selectedItem);
            }
        }
    }
}