using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using GameCenterV2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameCenterV2.ViewModels
{
    public partial class ItemListViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly ItemServices _itemService;
        private TbItem _item;
        [ObservableProperty] ObservableCollection<TbItem> crossSellitems;       
        public TbItem item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        bool isFirstRun;
        [ObservableProperty]
        private int itemId;
       
        public ItemListViewModel(ItemServices itemService, AuthService authService)
        {
            _authService = authService;
            _itemService = itemService;
            NavigateToAdditemCommand = new AsyncRelayCommand(NavigateToAdditemPageAsync);            
            isFirstRun = true;
            SearchCommand = new Command(PerformSearch);
            CrossSellitems = new ObservableCollection<TbItem>();
            GetitemAsync();
        }


        [RelayCommand]
        private async Task ViewImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                await Application.Current.MainPage.DisplayAlert("عذرا", "الصورة غير متاحة.", "موافق");
                return;
            }

            // Create an ImageSource from the byte array
            var imageSource = ImageSource.FromStream(() => new MemoryStream(imageData));


            var imagePage = new ImagePage(imageSource);
            await Application.Current.MainPage.Navigation.PushModalAsync(imagePage);
        }
        public IAsyncRelayCommand NavigateToAdditemCommand { get; }
        private async Task NavigateToAdditemPageAsync()
        {
            try
            {
                //var viewModel = new AddItemViewModel(_itemService);
                await Shell.Current.GoToAsync(nameof(AddItemPage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to navigate to Add item Page.", "OK");
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="itemViewModel"/> class.
        /// </summary>
        public ItemListViewModel()
        {
        }
        [ObservableProperty]
        public bool isUserLoggedIn;

        /// <summary>
        /// Gets or sets a value indicating whether the view model is busy with cart modification.
        /// </summary>
        [ObservableProperty]
        private bool isBusyWithitemModification;

        public async Task ShowOptionsForItemAsync(TbItem item)
        {
            if (item == null)
            {
                await Application.Current.MainPage.DisplayAlert("خطأ", "لم يتم اختيار مادة", "حسنًا");
                return;
            }

            string action = await Application.Current.MainPage.DisplayActionSheet($"خيارات المادة: {item.IName}", "إلغاء", null, "تعديل", "حذف");

            switch (action)
            {
                case "تعديل":
                    await Edititem(item);
                    break;
                case "حذف":
                    await Deleteitem(item);
                    break;
            }
        }

        private async Task Edititem(TbItem item)
        {
            var viewModel = new AddItemViewModel(_itemService)
            {

                IId = item.IId,
                IName = item.IName,
                IQty = item.IQty,
                IPriceSale = item.IPriceSale,
                IPriceBuy = item.IPriceBuy,
                IStoreId = item.IStoreId,
                IImg = item.IImg,
                IIstime = item.IIstime,
                IPrint = item.IPrint,
                IOrder = item.IOrder,

            };
            await Shell.Current.GoToAsync(nameof(AddItemPage), new Dictionary<string, object>
            {
                { "ViewModel", viewModel }
            });
        }

        private async Task Deleteitem(TbItem item)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("تأكيد الحذف", $"هل أنت متأكد من حذف المادة {item.IName}؟", "نعم", "لا");
            if (answer)
            {
                try
                {
                    await _itemService.DeleteItem(item.IId);
                    CrossSellitems.Remove(item);
                    // await Application.Current.MainPage.DisplayAlert("تم الحذف", "تم حذف الزبون بنجاح.", "موافق");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ أثناء حذف المادة.", "موافق");
                }

            }
        }




        /// <summary>
        /// Initializes the view model.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        
        public async Task GetitemAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var item = await _itemService.GetItemAsync();
                CrossSellitems.Clear();
                _allitems.Clear();
                if (item != null && item.Any())
                {
                     foreach (var custome in item)
                    {
                        CrossSellitems.Add(custome);

                        _allitems.Add(custome);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get item.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        /// <summary>
        /// Handles the item tapped event.
        /// </summary>
        /// <param name="item">The tapped item.</param>

        [RelayCommand]
        private async Task itemTappedAsync(TbItem item)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("اختر إجراء", "إلغاء", null, "تعديل", "حذف");

            if (action == "تعديل")
            {
                await Edititem(item);
            }
            else if (action == "حذف")
            {
                await Deleteitem(item);
            }
        }
        

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                SearchCommand.Execute(null);
            }
        }
        public ICommand SearchCommand { get; }
        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // If search query is empty, show all items
                CrossSellitems = new ObservableCollection<TbItem>(_allitems);
            }
            else
            {
                CrossSellitems.Clear();
                //Filter items based on search query
                var filtereditems = _allitems.Where(c =>
            (c.IName != null && c.IName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))).ToList();


                CrossSellitems = new ObservableCollection<TbItem>(filtereditems);
            }
        }
        private List<TbItem> _allitems = new List<TbItem>();
    }
}
