using GameCenterV2.Models;
using GameCenterV2.Services;
using GameCenterV2.ViewModels;
using GameCenterV2.Views;
using System.Net.Http;

namespace GameCenterV2
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }
        private  AuthService _authService;
        public App(IServiceProvider services,AuthService authService)
        {
            Services = services;
            _authService = authService;
            _authService.IsUserLoggedIn = false;
            InitializeComponent();
            MainPage = new AppShell(_authService);

            // التحقق من عنوان السيرفر
            //Preferences.Remove("DatabaseIpAddress");

            var ipAddress = Preferences.Get("DatabaseIpAddress", null);
            if (!string.IsNullOrEmpty(ipAddress))
            {
                
                // تحقق من الاتصال
                bool isConnected = TestApiConnection(ipAddress).Result;
                if (isConnected)
                {
                    //LoginViewModel loginViewModel = new LoginViewModel();
                   // MainPage = new LoginPage(loginViewModel);
                    MainPage = new AppShell(_authService); // يذهب مباشرة إلى صفحة تسجيل الدخول
                }
                else
                {
                    MainPage = new NavigationPage(new EnterIpPage(_authService));
                }
            }
            else
            {
                MainPage = new NavigationPage(new EnterIpPage(_authService));
            }
            //MainPage = new NavigationPage(Services.GetService<LoginPage>());
        }

        private async Task<bool> TestApiConnection(string ipAddress)
        {
            try
            {
                // افترض أن لديك خدمة API للـ health check
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.BaseAddress = new Uri($"https://{ipAddress}:5244");
                    //var response = await client.GetAsync("http://localhost:5244/api/HealthCheck");
                    HttpResponseMessage response = await client.GetAsync("api/HealthCheck").ConfigureAwait(false);

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
