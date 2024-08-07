using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameCenterV2.ViewModels
{
    public partial class OpenBoxViewModel:BaseViewModel
    {
        [ObservableProperty] int boxId;
        [ObservableProperty] DateTime boxDateIn;
        [ObservableProperty] DateTime? boxDateOut;
        [ObservableProperty] double boxMonyIn;
        [ObservableProperty] double boxMonyOut;

        [ObservableProperty] string boxDetails;
        [ObservableProperty] int? boxUId;
        [ObservableProperty] bool boxIsopen;
        [ObservableProperty] bool showClose;
        [ObservableProperty] bool showOpen = true;


        private readonly BoxServices _boxServices;
        public OpenBoxViewModel(BoxServices boxServices)
        {
            _boxServices = boxServices;
            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync);
            this.SaveAsyncCommand = SaveAsyncCommand;
            CloseAsyncCommand = new AsyncRelayCommand(CloseAsync);
            this.CloseAsyncCommand = CloseAsyncCommand;
            //عند استدعاء الموديل حتى يملئ المتغيرات بالبياانت
            GetOpenBoxId();
        }
        
        public ICommand SaveAsyncCommand { get; }
        public ICommand CloseAsyncCommand { get; }

        [RelayCommand]
        private async Task SaveAsync()
        {

            
            bool isSuccess = false;
            // Call the service to save the customer
            if (BoxId == 0)
            {
                var newBox = new TbBox
                {
                    BoxId = boxId,
                    BoxDateIn = DateTime.Now,
                    BoxDateOut = boxDateOut,
                    BoxMonyIn = BoxMonyIn,
                    BoxDetails = boxDetails,
                    BoxUId = PublicVariables._CurrentUserId,
                    BoxIsopen = true,
                };
                isSuccess = await _boxServices.AddBoxAsync(newBox);
                if (isSuccess)
                {
                    GetOpenBoxId();
                    await Shell.Current.DisplayAlert("Success", "تم فتح الصندوق", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء فتح الصندوق", "OK");
                }
            } 
            else
            {
                await Shell.Current.DisplayAlert("Error", "لم يتم فتح الصندوق", "OK");

            }

            // أغلق النافذة المنبثقة
            await Shell.Current.GoToAsync("..");
        }
        private async void GetOpenBoxId()
        {
            var boxes = await _boxServices.GetBoxAsync();
            bool isopen = false;
            Int32 UID = 0;
            foreach (var box in boxes)
            {
                if (box != null)
                {
                    isopen = box.BoxIsopen;
                    if (isopen)
                    {
                        UID = box.BoxUId ?? 0;
                        PublicVariables.BoxID = box.BoxId;
                        PublicVariables.OpenBox = box.BoxIsopen;
                        BoxMonyIn = BoxMonyIn;
                        BoxDateIn = BoxDateIn;
                        BoxDetails = BoxDetails;

                    }
                    break;
                }
            }
        }
        [RelayCommand]
        private async Task CloseAsync()
        {


            bool isSuccess = false;
            if (BoxId != 0)
            {
                var newBox = new TbBox
                {
                    BoxId = BoxId,
                    BoxDateIn = BoxDateIn,
                    BoxDateOut = DateTime.Now,
                    BoxMonyOut = BoxMonyOut,
                    BoxMonyIn = BoxMonyIn,
                    BoxDetails = BoxDetails,
                    BoxUId = BoxUId,
                    BoxIsopen = false,
                };
                isSuccess = await _boxServices.UpdateBox(newBox);
                if (isSuccess)
                {
                    await Shell.Current.DisplayAlert("Success", "تم اغلاق الصندوق", "OK");
                    Application.Current.Quit();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء فتح الصندوق", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "لم يتم الاغلاق", "OK");
            }
            
            
            // أغلق النافذة المنبثقة
            await Shell.Current.GoToAsync("..");
        }
    }
}
