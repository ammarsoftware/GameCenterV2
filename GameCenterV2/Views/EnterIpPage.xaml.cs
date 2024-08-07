using GameCenterV2.Models;
using GameCenterV2.Services;
using System.Diagnostics;

namespace GameCenterV2.Views;

public partial class EnterIpPage : ContentPage
{
    private readonly AuthService _authService;
    public EnterIpPage(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
	}
    private async void OnConnectClicked(object sender, EventArgs e)
    {
        try
        {
            string ipAddress = IpEntry.Text;

            // Validate the IP address format here if needed

            // Save the IP address to Preferences or SecureStorage
            Preferences.Set("DatabaseIpAddress", ipAddress);

            // Test the connection to the database
            bool isConnected = await TestApiConnection(ipAddress).ConfigureAwait(false);

            if (isConnected)
            {
                // Save the IP address to Preferences
                Preferences.Set("DatabaseIpAddress", ipAddress);
                //Application.Current.MainPage = new AppShell(_authService);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AppShell(_authService);
                });
                 await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await DisplayAlert("Error", "Unable to connect to the database. Please check the IP address and try again.", "OK");
            }
        }
        catch (Exception ex)
        {
           // Debug.Write(ex.Message);
        }
    }

    
    private async Task<bool> TestApiConnection(string ipAddress)
    {
        try
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var response = await httpClient.GetAsync($"https://{ipAddress}:5244/api/HealthCheck").ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
           
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}