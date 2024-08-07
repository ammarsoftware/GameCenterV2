using GameCenterV2.Services;
using GameCenterV2.ViewModels;
using GameCenterV2.Views;

namespace GameCenterV2
{
    public partial class AppShell : Shell
    {
        private AuthService _authService;
        public AppShell(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            FlowDirection = FlowDirection.LeftToRight;

            // Register routes
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
            Routing.RegisterRoute(nameof(AddEditTablePage), typeof(AddEditTablePage));
            Routing.RegisterRoute(nameof(PosPage), typeof(PosPage));
            Routing.RegisterRoute(nameof(OpenBoxPage), typeof(OpenBoxPage));
            Routing.RegisterRoute(nameof(ItemListPage), typeof(ItemListPage));
            Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
            Routing.RegisterRoute(nameof(AddStorePage), typeof(AddStorePage));
            Routing.RegisterRoute(nameof(StoreListPage), typeof(StoreListPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ReceiptPage), typeof(ReceiptPage));
            Routing.RegisterRoute(nameof(EnterIpPage), typeof(EnterIpPage));
            Routing.RegisterRoute(nameof(ImagePage), typeof(ImagePage));
            Routing.RegisterRoute(nameof(MergeTablesPage), typeof(MergeTablesPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

           // CheckLoginStatus();
        }
        private async void CheckLoginStatus()
        {
            if (!_authService.IsUserLoggedIn)
            {
                // إذا لم يكن المستخدم مسجل الدخول، قم بتوجيهه إلى صفحة تسجيل الدخول
               // await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        protected override void OnAppearing()
        {
           base.OnAppearing();

            if (!_authService.IsUserLoggedIn)
            {
                Shell.Current.Dispatcher.Dispatch(async () =>
                {
                    await  Shell.Current.GoToAsync(nameof(LoginPage));
                });
            }
        }
        // طريقة للتحقق من تسجيل الدخول قبل الانتقال إلى أي صفحة
        //protected override async void OnNavigating(ShellNavigatingEventArgs args)
        //{
        //    base.OnNavigating(args);
        //    try
        //    {
        //        // تحقق من تسجيل الدخول قبل كل عملية تنقل
        //        if (!args.Target.Location.ToString().Contains("LoginPage"))
        //        {
        //            _authService = new AuthService();
        //            if (!_authService.IsUserLoggedIn)
        //            {
        //                args.Cancel();
        //                await Shell.Current.GoToAsync(nameof(LoginPage));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }
       // }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("تسجيل الخروج", "هل انت متأكد?", "Yes", "No");

            if (result)
            {
                _authService.IsUserLoggedIn = false;

                // Navigate to login page
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        }
        private async void OnCloseBoxClicked(object sender, EventArgs e)
        {
            BoxServices box = new BoxServices();
            var viewModel = new OpenBoxViewModel(box)
            {
                ShowClose = true,
                ShowOpen = false,
                BoxId = PublicVariables.BoxID,
                BoxUId = PublicVariables._CurrentUserId
            };
            await Shell.Current.GoToAsync(nameof(OpenBoxPage), new Dictionary<string, object>
            {
                { "ViewModel", viewModel }
            });
        }
    }
}
