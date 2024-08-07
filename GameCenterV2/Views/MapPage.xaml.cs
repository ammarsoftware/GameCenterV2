using GameCenterV2.Models;
using GameCenterV2.ViewModels;

namespace GameCenterV2.Views;

public partial class MapPage : ContentPage
{
	public MapPage(MapViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var collectionView = this.FindByName<CollectionView>("YourCollectionViewName");
        System.Diagnostics.Debug.WriteLine($"MapPage: عدد العناصر في CollectionView = {collectionView?.ItemsSource?.Cast<object>().Count() ?? 0}");
    }
    //private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (e.CurrentSelection.FirstOrDefault() is TbTable selectedCustomer)
    //    {
    //        // ÅÚÇÏÉ ÊÚííä ÇáÇÎÊíÇÑ
    //        ((CollectionView)sender).SelectedItem = null;

    //        // ÇÓÊÏÚÇÁ ÇáÃãÑ Ýí ViewModel
    //        if (BindingContext is MapViewModel viewModel)
    //        {
    //            await viewModel.ShowOptionsForTableAsync(selectedCustomer);
    //        }
    //    }
    //}
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("MapPage: محاولة النقر على الطاولة");
        if (e.CurrentSelection.FirstOrDefault() is TableShow selectedTable)
        {
            // ÇÓÊÏÚÇÁ ÇáÃãÑ Ýí ViewModel
            if (BindingContext is MapViewModel viewModel)
            {
                viewModel.TableTappedCommand.Execute(selectedTable);
            }

            // ÅáÛÇÁ ÊÍÏíÏ ÇáÚäÕÑ ÈÚÏ ãÚÇáÌÊå
            ((CollectionView)sender).SelectedItem = null;
        }
        //if (e.CurrentSelection.FirstOrDefault() is TableShow selectedTable)
        //{
        //    // ÅÚÇÏÉ ÊÚííä ÇáÇÎÊíÇÑ
        //    ((CollectionView)sender).SelectedItem = null;

        //    // ÇÓÊÏÚÇÁ ÇáÃãÑ Ýí ViewModel
        //    if (BindingContext is MapViewModel viewModel)
        //    {
        //        //await viewModel.ShowOptionsForTableAsync(selectedTable);
        //        //await viewModel.GoToPosPage();
        //    }
        //}
    }


}