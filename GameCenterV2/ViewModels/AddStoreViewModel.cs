using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameCenterV2.ViewModels
{
    public partial class AddStoreViewModel:BaseViewModel
    {
        private readonly StoreServices _storeService;
        public AddStoreViewModel(StoreServices storeService)
        {
            _storeService = storeService;
            
            PickStoreImageAsyncCommand = new AsyncRelayCommand(PickStoreImageAsync);
            this.PickStoreImageAsyncCommand = PickStoreImageAsyncCommand;

            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync);
            this.SaveAsyncCommand = SaveAsyncCommand;
        }
        public AddStoreViewModel()
        {
        }
        [ObservableProperty] int storeId;
        [ObservableProperty] string? storeName;
        [ObservableProperty] int? storeOrder;
        [ObservableProperty] byte[]? storeImg;
        public ICommand PickStoreImageAsyncCommand { get; set; }
        public ICommand SaveAsyncCommand { get; }

        [RelayCommand]
        private async Task PickStoreImageAsync()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Please select an image"
            });
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                StoreImg = ReadFully(stream);
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
            var newStore = new TbStore
            {
                StoreId = StoreId,
                StoreImg = ResizeImage(StoreImg, 300, 300),
                StoreName = StoreName,               
                StoreOrder = StoreOrder,
            };
            bool isSuccess = false;
            //Call the service to save the customer
            if (StoreId == 0)
            {
                isSuccess = await _storeService.AddStoreAsync(newStore);

            }
            else
            {
                isSuccess = await _storeService.UpdateStore(newStore);

            }
            if (isSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "تم إضافة القسم", "OK");
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء إضافة القسم", "OK");
            }
        }
    }
}
