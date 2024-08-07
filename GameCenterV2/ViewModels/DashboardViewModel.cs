using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;
using GameCenterV2.Services;

namespace GameCenterV2.ViewModels
{
  
    public partial class DashboardViewModel : ObservableObject
    {
        ItemServices _itemservice = new ItemServices();
        MenuServices _menuServices = new MenuServices();
        [ObservableProperty]
        private int _availableMeals;
        //public int AvailableMeals
        //{
        //    get => _availableMeals;
        //    set => SetProperty(ref _availableMeals, value);
        //}
        [ObservableProperty]
        private decimal _currentBalance;
        //public decimal CurrentBalance
        //{
        //    get => _currentBalance;
        //    set => SetProperty(ref _currentBalance, value);
        //}
        [ObservableProperty]
        private int _pingValue;
        //public int PingValue
        //{
        //    get => _pingValue;
        //    set => SetProperty(ref _pingValue, value);
        //}
        [ObservableProperty]
        private string _serverAddress = Preferences.Get("DatabaseIpAddress",null);
        //public ICommand RefreshCommand { get; }
        public DashboardViewModel()
        {
            //RefreshCommand = new RelayCommand(RefreshData);
            Refresh();
        }

        [RelayCommand]
        private async void Refresh()
        {
            await UpdateAvailableMeals();
            await UpdateCurrentBalance();
            await UpdatePingValue();
        }
        private async Task UpdateAvailableMeals()
        {
            try
            {
                AvailableMeals = await _itemservice.GetMealCountAsync();
            }
            catch (Exception ex)
            {
                // يمكنك التعامل مع الخطأ هنا، مثلاً عرض رسالة للمستخدم
                //Console.WriteLine($"Error updating available meals: {ex.Message}");
                AvailableMeals = 0;
            }
        }

        private async Task UpdateCurrentBalance()
        {
            try
            {
                CurrentBalance = await _menuServices.GetMonyInBoxAsync(PublicVariables.BoxID); 
            }
            catch (Exception ex)
            {
                // التعامل مع الخطأ (مثلاً عرض رسالة للمستخدم)
                Console.WriteLine($"Error fetching box balance: {ex.Message}");
            }
        }

        private async Task UpdatePingValue()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(ServerAddress);
                    if (reply.Status == IPStatus.Success)
                    {
                        PingValue = (int)reply.RoundtripTime;
                    }
                    else
                    {
                        PingValue = -1; // قيمة تشير إلى فشل الاتصال
                    }
                }
            }
            catch (Exception)
            {
                PingValue = -1; // قيمة تشير إلى حدوث خطأ
            }
        }
    }
}
