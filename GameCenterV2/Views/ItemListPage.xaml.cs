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
    //������ �������� ��� ������� �� �������
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // ����� �������� ��� ���� ������
        await _viewmodel.GetitemAsync(); // ���� �� �� ���� ���� ������ ��������
    }
    private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TbItem selectedItem)
        {
            // ����� ����� ��������
            ((CollectionView)sender).SelectedItem = null;

            // ������� ����� �� ViewModel
            if (BindingContext is ItemListViewModel viewModel)
            {
                await viewModel.ShowOptionsForItemAsync(selectedItem);
            }
        }
    }
}