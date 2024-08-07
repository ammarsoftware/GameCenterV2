using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameCenterV2.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using GameCenterV2.Services;
using System.Windows.Input;
using System.Security.Cryptography;
using GameCenterV2.SubView;
using Mopups.Services;
using GameCenterV2.Views;
using iText.Layout.Element;


namespace GameCenterV2.ViewModels
{
    public partial class PosViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<TbStore> foodCategories;

        [ObservableProperty]
        private ObservableCollection<TbItem> foodItems;

        [ObservableProperty]
        private ObservableCollection<TbService> selectedItems;
        [ObservableProperty] int quantity;
        [ObservableProperty] private double? totalPrice;
        [ObservableProperty] int menuNumber;
        [ObservableProperty] private int? menuid;
        [ObservableProperty] private int? tableid;
        [ObservableProperty] bool isQuickPos = false;
        private readonly StoreServices _storeServices;
        private readonly ItemServices _itemServices;
        private readonly MenuServices menuServices;// = new MenuServices();
        private readonly ServiceServices serviceServices;//= new ServiceServices();

     
        public PosViewModel()//StoreServices storeServices, ItemServices itemServices)
        {
            FoodCategories = new ObservableCollection<TbStore>();
            FoodItems = new ObservableCollection<TbItem>();
            SelectedItems = new ObservableCollection<TbService>();
            _storeServices = new StoreServices();// storeServices;
            _itemServices = new ItemServices();// itemServices;
            menuServices = new MenuServices();
            serviceServices = new ServiceServices();
            GetStoreAsync();            
            AutoNumber();            
        }
        partial void OnMenuidChanged(int? value)
        {
             if (value != null)
            {
                ControlView();                
            }
        }
        partial void OnTableidChanged(int? value)
        {
            if (value != null)
            {
                CheckQuickPos();
            }
        }
        private async Task GetStoreAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var table = await _storeServices.GetStoreAsync();
                FoodCategories.Clear();
                if (table != null && table.Any())
                {
                    // قم بتحويل القائمة إلى ObservableCollection
                    foreach (var custome in table)
                    {
                        FoodCategories.Add(custome);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get customer.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
       
        [RelayCommand]
        private async void LoadFoodItems(string category)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var table = await _itemServices.GetItemAsync();
                FoodItems.Clear();
                if (table != null && table.Any())
                {
                    // قم بتحويل القائمة إلى ObservableCollection
                    foreach (var custome in table)
                    {
                        if (category == custome.IStore.StoreName)
                        {
                            FoodItems.Add(custome);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get customer.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private void AddToOrder(TbItem item)
        {
            var existingItem = SelectedItems.FirstOrDefault(i => i.SeI.IName == item.IName);
            if (existingItem != null)
            {
                existingItem.SeQty++;
            }
            else
            {
                int qty = item.IQty??1;                
                SelectedItems.Add(new TbService { 
                    SeI = item, 
                    SePrice = item.IPriceSale, 
                    SeQty = qty ,
                    SeTNumber = Tableid,
                });
            }
            CalculateTotalPrice();
        }
        private void AddToOrderControlView(TbService service)
        {
            
                int qty = service.SeQty??1;
                int? serId = null;
                if (service.SeId != 0)
                    serId = service.SeId;
                SelectedItems.Add(new TbService { 
                    SeI = service.SeI, 
                    SePrice = service.SePrice, 
                    SeQty = qty, 
                    SeId = serId??0, 
                    SeTNumber = Tableid });
            
            CalculateTotalPrice();
        }
        private void CalculateTotalPrice()
        {
            TotalPrice = SelectedItems.Sum(item => item.SePrice * item.SeQty);
        }
        [RelayCommand]
        private async Task RemoveOrder(TbService item)
        {
            if (item != null && SelectedItems.Contains(item))
            {
                //يجب حذفها من السيرفس ايضا
                await DeleteItem(item.SeId);
                SelectedItems.Remove(item);
                CalculateTotalPrice();
            }
        }
        
        private void AutoNumber()
        {
           MenuNumber = menuServices.AutoNumber().Result;
        }
        private async Task DeleteItem(int serviceId)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("تأكيد الحذف", $"هل أنت متأكد من الحذف؟", "نعم", "لا");
            if (answer)
            {
                try
                {
                    await serviceServices.DeleteService(serviceId);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("خطأ", "حدث خطأ أثناء الحذف.", "موافق");
                }

            }
        }
        private async Task<bool> SaveMenu(bool isActive)
        {
            try
            {
                var newmenu = new TbMenu
                {
                    MId = MenuNumber,
                    MBoxId = PublicVariables.BoxID,
                    MDate = DateTime.Now,
                    MUId = PublicVariables._CurrentUserId,
                    MIsactive = isActive,
                    
                };
                bool isSuccess = false;
                // Call the service to save the customer
                if (Menuid == null)
                {
                    isSuccess = await menuServices.AddMenuAsync(newmenu);
                }
                else
                {
                    isSuccess = await menuServices.UpdateMenu(newmenu);

                }
                if (isSuccess)
                {
                    //await Shell.Current.DisplayAlert("Success", "تم إضافة الطاولة بنجاح", "OK");
                    //await Shell.Current.GoToAsync("..");
                }
                else
                {
                    //await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء إضافة الطاولة", "OK");
                }
                return isSuccess;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> SaveServices()
        {
            try
            {
                // تجميع الطلبات في قائمة
                var orderItems = SelectedItems.Select(selectedItem => new TbService
                {
                    SeId = selectedItem.SeId,
                    SeMenuId = MenuNumber,
                    SeIId = selectedItem.SeI.IId,
                    SePrice = selectedItem.SePrice,
                    SeQty = selectedItem.SeQty,
                    SeTNumber = selectedItem.SeTNumber,
                }).ToList();

                // استدعاء الخدمة لحفظ الطلبات
                bool isSuccess = await serviceServices.AddOrderItemsAsync(orderItems);

                if (isSuccess)
                {
                    //await Shell.Current.DisplayAlert("Success", "تم إضافة الطلبات بنجاح", "OK");
                }
                else
                {
                   // await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء إضافة الطلبات", "OK");
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء حفظ الطلبات.", "OK");
                return false;
            }
        }
        private async void SaveMenuOrder(bool isactive)
        {
            if (await SaveMenu(isactive))
            {
                if (await SaveServices())
                {
                    await Shell.Current.DisplayAlert("ok", "تم حفظ الطلبات.", "OK");
                    NewMenu();
                }
            }
        }
        private void CheckQuickPos()
        {
            if(Tableid != null)
            {
                //البيع السريع
                IsQuickPos = true;
            }
        }
        private void NewMenu()
        {
            AutoNumber();
           selectedItems.Clear();
            CalculateTotalPrice();
        }
        private async Task Printer()
        {
            try
            {
                var orderItems = SelectedItems.Select(selectedItem => new TbService
                {
                    SeId = selectedItem.SeId,
                    SeMenuId = MenuNumber,
                    SeIId = selectedItem.SeI.IId,
                    SePrice = selectedItem.SePrice,
                    SeQty = selectedItem.SeQty,
                    SeTNumber = selectedItem.SeTNumber,
                    SeI = selectedItem.SeI  // تأكد من تضمين هذا إذا كنت تريد عرض اسم العنصر
                }).ToList();
                
                
                await Shell.Current.Navigation.PushAsync(new ReceiptPage(orderItems));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"حدث خطأ: {ex.Message}");
                Console.WriteLine($"تفاصيل الخطأ: {ex.StackTrace}");
                // يمكنك هنا إظهار رسالة خطأ للمستخدم إذا كنت ترغب في ذلك
            }
        }
       
        [RelayCommand]
        private async Task PrintOrder()
        {
            if (Tableid != null)
                SaveMenuOrder(true);
            else
                SaveMenuOrder(false);
            Console.WriteLine("بدء عملية الطباعة");
            await Printer();
        }
        [RelayCommand]
        private async Task CloseMenu()
        {
            SaveMenuOrder(false);
        }
        private async Task ControlView()
        {
            //ناتي بكل الطلبات من قائمة الطلبات
            var serviceee = await serviceServices.GetServiceAsync(Menuid);
            MenuNumber = Menuid??0;
            // كل الطلبات التي في services يجب ان تتحول الى 
            foreach (var service in serviceee)
            {
                // Extract the TbItem from the service and add it to the order
                if (service.SeI != null)
                {
                    AddToOrderControlView(service);
                }
            }
        }


        [RelayCommand]
        private async void ShowQuantityPopup(TbService orderItem)
        {
            var popup = new QuantityPopup(orderItem.SeQty.ToString());
            await MopupService.Instance.PushAsync(popup);

            MessagingCenter.Subscribe<QuantityPopupViewModel, string>(this, "QuantityUpdated", (sender, quantity) =>
            {
                if (int.TryParse(quantity, out int newQuantity))
                {
                    orderItem.SeQty = newQuantity;
                    CalculateTotalPrice();
                }
                MessagingCenter.Unsubscribe<QuantityPopupViewModel, string>(this, "QuantityUpdated");
            });
        }
        [RelayCommand]
        private async void ShowPricePopup(TbService orderItem)
        {
            var popup = new PricePopup(orderItem.SePrice.ToString());
            await MopupService.Instance.PushAsync(popup);

            MessagingCenter.Subscribe<PricePopupViewModel, string>(this, "PriceUpdated", (sender, price) =>
            {
                if (double.TryParse(price, out double newPrice))
                {
                    orderItem.SePrice = newPrice;
                    CalculateTotalPrice();
                }
                MessagingCenter.Unsubscribe<PricePopupViewModel, string>(this, "PriceUpdated");
            });
        }

        // Merage Table
        //
        //
        [RelayCommand]
        private async Task MergeTables()
        {
            var viewModel = new MergeTablesViewModel()
            {
                OrginalTable = Tableid??0,
                MenuId = Menuid??0,
                CancelMerge = false,
                MoveTable = false
            };
            await Shell.Current.GoToAsync(nameof(MergeTablesPage), new Dictionary<string, object>
            {
                { "ViewModel", viewModel }
            });
        }
        //
        // Cancel Merage Table
        //
        [RelayCommand]
        private async Task CancelMergeTables()
        {
            var viewModel = new MergeTablesViewModel()
            {
                OrginalTable = Tableid ?? 0,
                MenuId = Menuid ?? 0,
                CancelMerge = true,
                MoveTable = false,
            };
            await Shell.Current.GoToAsync(nameof(MergeTablesPage), new Dictionary<string, object>
            {
                { "ViewModel", viewModel }
            });
        }
        //
        // Move Table
        //
        [RelayCommand]
        private async Task MoveTables()
        {
            var viewModel = new MergeTablesViewModel()
            {
                OrginalTable = Tableid ?? 0,
                MenuId = Menuid ?? 0,
                CancelMerge = false,
                MoveTable =  true
            };
            await Shell.Current.GoToAsync(nameof(MergeTablesPage), new Dictionary<string, object>
            {
                { "ViewModel", viewModel }
            });
        }

    }
}
