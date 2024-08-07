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
    public partial class AddTableViewModel : ObservableObject
    {
        private readonly MapServices _tableService;

        public AddTableViewModel(MapServices mapServices)
        {
            _tableService = mapServices;
            PickTableImageAsyncCommand = new AsyncRelayCommand(PickTableImageAsync);
            this.PickTableImageAsyncCommand = PickTableImageAsyncCommand;

            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync);
            this.SaveAsyncCommand = SaveAsyncCommand;
        }
        public AddTableViewModel()
        {
            PickTableImageAsyncCommand = new AsyncRelayCommand(PickTableImageAsync);
            this.PickTableImageAsyncCommand = PickTableImageAsyncCommand;

            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync);
            this.SaveAsyncCommand = SaveAsyncCommand;
        
        }


        [ObservableProperty]
        private int tid;
        [ObservableProperty]
        private string tName ;
        [ObservableProperty]
        private int tNumber;
        [ObservableProperty]
        private string? tDetails;
        [ObservableProperty]
        private string? tLocation;
        [ObservableProperty]
        private int? tMap;
        [ObservableProperty]
        private byte[]? tbImage;
        [ObservableProperty]
        private int? tDefaultItem;

        private byte[] _tableImage;
        public byte[] TableImage
        {
            get => _tableImage;
            set
            {
                _tableImage = value;
                OnPropertyChanged(nameof(TableImage));
                OnPropertyChanged(nameof(TableImageSource));
            }
        }
        public ImageSource TableImageSource
        {
            get
            {
                if (_tableImage != null && _tableImage.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(_tableImage));
                }
                return null;
            }
            private set => OnPropertyChanged(nameof(TableImageSource));
        }

        
        public ICommand PickTableImageAsyncCommand { get; set; }
        public ICommand SaveAsyncCommand { get; }
        [RelayCommand]
        private async Task PickTableImageAsync()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Please select an image"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                _tableImage = ReadFully(stream);
                TableImageSource = ImageSource.FromStream(() => new MemoryStream(_tableImage));
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
            var newTable = new TbTable
            {
                TId = tid,
                TName = TName,
                TNumber = TNumber,
                TLocation = TLocation,
                TMap = TMap,                
                TbImage = ResizeImage(_tableImage, 400, 400), // يمكنك تعديل الأبعاد حسب الحاجة
                
            };
            bool isSuccess = false;
            // Call the service to save the customer
            if (Tid == 0)
            {
                isSuccess = await _tableService.AddTableAsync(newTable);
            }
            else
            {
                isSuccess = await _tableService.UpdateTable(newTable);

            }
            if (isSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "تم إضافة الطاولة بنجاح", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء إضافة الطاولة", "OK");
            }
        }
    }
}
