using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using SkiaSharp;
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
    
    public partial class AddItemViewModel : BaseViewModel
    {
       
        private readonly ItemServices _itemService;
        private StoreServices _storeService = new StoreServices();
        public AddItemViewModel(ItemServices itemService)
        {
            _itemService = itemService;
            Departments = new ObservableCollection<TbStore>();           
            GetStoreAsync();
        }
        public AddItemViewModel()
        {
        }
        private async Task GetStoreAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                var table = await _storeService.GetStoreAsync();
                Departments.Clear();
                if (table != null && table.Any())
                {
                    foreach (var custome in table)
                    {
                        Departments.Add(custome);
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
        public ObservableCollection<TbStore> Departments { get; set; }
        [ObservableProperty] TbStore selectedDepartment;
        [ObservableProperty] int? itemId;
        [ObservableProperty] string? iName;
        [ObservableProperty] int? iQty;
        [ObservableProperty] float? iPriceSale;
        [ObservableProperty] int iId;
        [ObservableProperty] float? iPriceBuy ;
        [ObservableProperty] int? iStoreId;
        [ObservableProperty] byte[]? iImg;
        [ObservableProperty] bool iIstime;
        [ObservableProperty] bool iPrint;
        [ObservableProperty] int? iOrder;

      
        [RelayCommand]
        private async Task PickItemImageAsync()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Please select an image"
            });
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                IImg = ReadFully(stream);
            }
        }
       
        public byte[] ResizeImage(byte[] imageData, int width, int height)
        {
            if (imageData != null)
            {
                using var inputStream = new MemoryStream(imageData);
                using var original = SKBitmap.Decode(inputStream);

                var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);
                using var image = SKImage.FromBitmap(resized);
                using var outputStream = new MemoryStream();
                image.Encode(SKEncodedImageFormat.Jpeg, 80).SaveTo(outputStream);
                return outputStream.ToArray();
            }
            else
                return null;
        }
        [RelayCommand]
        public async Task SaveAsync()
        {
            if (SelectedDepartment != null)
            {
                int selectedDepartmentId = this.SelectedDepartment.StoreId;
                var newCustomer = new TbItem
                {
                    IId = IId,
                    IImg = ResizeImage(IImg, 300, 300),
                    IIstime = IIstime,
                    IName = IName,
                    IOrder = IOrder,
                    IPrint = IPrint,
                    IPriceBuy = IPriceBuy,
                    IPriceSale = IPriceSale,
                    IQty = IQty,
                    IStoreId = selectedDepartmentId,
                };
                bool isSuccess = false;
                //Call the service to save the customer
                if (IId == null || IId == 0)
                {
                    isSuccess = await _itemService.AddItemAsync(newCustomer);

                }
                else
                {
                    isSuccess = await _itemService.UpdateItem(newCustomer);

                }
                if (isSuccess)
                {
                    await Shell.Current.DisplayAlert("Success", "تم إضافة المادة", "OK");
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء إضافة المادة", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("ملاحظة", "يجب تحديد قسم", "OK");

            }
        }

        private byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
