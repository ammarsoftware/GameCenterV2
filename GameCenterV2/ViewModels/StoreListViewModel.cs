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

namespace GameCenterV2.ViewModels
{
    public partial class StoreListViewModel:BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly StoreServices _storeService;
        [ObservableProperty] ObservableCollection<TbStore> crossStore;
       
        bool isFirstRun;
       
        public StoreListViewModel(StoreServices storeService, AuthService authService)
        {
            _authService = authService;
            _storeService = storeService;
            NavigateToAddStoreCommand = new AsyncRelayCommand(NavigateToAddStorePageAsync);
            isFirstRun = true;
            CrossStore = new ObservableCollection<TbStore>();
            GetStoreAsync();
        }


        
        public IAsyncRelayCommand NavigateToAddStoreCommand { get; }
        private async Task NavigateToAddStorePageAsync()
        {
            try
            {
                //var viewModel = new AddStoreViewModel(_storeService);
                await Shell.Current.GoToAsync(nameof(AddStorePage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to navigate to Add item Page.", "OK");
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="itemViewModel"/> class.
        /// </summary>
        public StoreListViewModel()
        {
        }
        [ObservableProperty]
        public bool isUserLoggedIn;

        /// <summary>
        /// Gets or sets a value indicating whether the view model is busy with cart modification.
        /// </summary>
        [ObservableProperty]
        private bool isBusyWithitemModification;

        public async Task ShowOptionsForItemAsync(TbStore item)
        {
            if (item == null)
            {
                await Application.Current.MainPage.DisplayAlert("خطأ", "لم يتم اختيار قسم", "حسنًا");
                return;
            }

            string action = await Application.Current.MainPage.DisplayActionSheet($"خيارات القسم: {item.StoreName}", "إلغاء", null, "تعديل", "حذف");

            switch (action)
            {
                case "تعديل":
                    await EditStore(item);
                    break;
                case "حذف":
                    await DeleteStore(item);
                    break;
            }
        }

        private async Task EditStore(TbStore store)
        {
            var viewModel = new AddStoreViewModel(_storeService)
            {
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                StoreOrder = store.StoreOrder,
                StoreImg = store.StoreImg,
            };
            await Shell.Current.GoToAsync(nameof(AddStorePage), new Dictionary<string, object>
            {
                { "ViewModel", viewModel }
            });
        }

        private async Task DeleteStore(TbStore store)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("تأكيد الحذف", $"هل أنت متأكد من حذف القسم {store.StoreName}؟", "نعم", "لا");
            if (answer)
            {
                try
                {
                    await _storeService.DeleteStore(store.StoreId);
                    CrossStore.Remove(store);
                    await Application.Current.MainPage.DisplayAlert("تم الحذف", "تم حذف القسم بنجاح.", "موافق");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ أثناء حذف القسم.", "موافق");
                }

            }
        }
        public async Task GetStoreAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var item = await _storeService.GetStoreAsync();
                CrossStore.Clear();
                if (item != null && item.Any())
                {
                    foreach (var custome in item)
                    {
                        CrossStore.Add(custome);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get store.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        /// <summary>
        /// Handles the item tapped event.
        /// </summary>
        /// <param name="store">The tapped item.</param>

        [RelayCommand]
        private async Task itemTappedAsync(TbStore store)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("اختر إجراء", "إلغاء", null, "تعديل", "حذف");

            if (action == "تعديل")
            {
                await EditStore(store);
            }
            else if (action == "حذف")
            {
                await DeleteStore(store);
            }
        }
       
    }
}
