using GameCenterV2.Services;
using GameCenterV2.ViewModels;
using GameCenterV2.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui.Core;

using GameCenterV2.Models;
using CommunityToolkit.Maui;
using Mopups.Hosting;
using CommunityToolkit.Maui.Storage;
using GameCenterV2.SubView;

namespace GameCenterV2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .ConfigureMopups()
                .UseMauiCommunityToolkit()
                  .ConfigureFonts(fonts =>
                  {
                      fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                      fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                      fonts.AddFont("astore-eczar-semi-bold.ttf", "astore-eczar-semi-bold");
                      
                      fonts.AddFont("times.ttf", "TimeNewRomain");
                      fonts.AddFont("timesbd.ttf", "TimeNewRomainBold");
                      fonts.AddFont("timesbi.ttf", "TimeNewRomainRegular");
                      fonts.AddFont("timesi.ttf", "TimeNewRomainI");

                  });
    
            //View
            builder.Services.AddTransient<EnterIpPage>();
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<OpenBoxPage>();
            builder.Services.AddTransient<AddEditTablePage>();
            builder.Services.AddTransient<PosPage>();
            builder.Services.AddTransient<ItemListPage>();
            builder.Services.AddTransient<AddItemPage>();
            builder.Services.AddTransient<AddStorePage>();
            builder.Services.AddTransient<StoreListPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<ReceiptPage>();
            builder.Services.AddTransient<ImagePage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<MergeTablesPage>();
            builder.Services.AddTransient<LoginPage>();



            builder.Services.AddSingleton(FileSaver.Default);

            //ViewModel
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MapViewModel>();
            builder.Services.AddTransient<AddTableViewModel>();
            builder.Services.AddTransient<PosViewModel>();
            builder.Services.AddTransient<OpenBoxViewModel>();
            builder.Services.AddTransient<ItemListViewModel>();
            builder.Services.AddTransient<AddItemViewModel>();
            builder.Services.AddTransient<AddStoreViewModel>();
            builder.Services.AddTransient<StoreListViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<ReceiptViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<MergeTablesViewModel>();


            //Services
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<MapServices>();
            builder.Services.AddSingleton<StoreServices>();
            builder.Services.AddSingleton<ItemServices>();
            builder.Services.AddSingleton<BoxServices>();
            builder.Services.AddSingleton<ServiceServices>();
            builder.Services.AddSingleton<MenuServices>();

            // تسجيل LoginPage مع تمرير LoginViewModel
            //builder.Services.AddTransient<LoginPage>( provider =>
            //{
            //    var loginViewModel = provider.GetRequiredService<LoginViewModel>();
            //    return new LoginPage(loginViewModel);
            //});
            //builder.Services.AddTransient<AppShell>(provider =>
            //{
            //    var authService = provider.GetRequiredService<AuthService>();
            //    return new AppShell(authService);
            //});

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
