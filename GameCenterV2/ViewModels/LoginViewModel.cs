using GameCenterV2.Models;
using GameCenterV2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GameCenterV2.Views;

namespace GameCenterV2.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private AuthService _authService;
        private readonly BoxServices _box;
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            _box = new BoxServices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class. This empty constructor is used for design-time data.
        /// </summary>
        public LoginViewModel()
        {
            _authService = new AuthService ();
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [ObservableProperty]
        string username;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [ObservableProperty]
        string password;

        /// <summary>
        /// Gets or sets a value indicating whether the password is visible.
        /// </summary>
        [ObservableProperty]
        bool isPasswordVisible;

        [ObservableProperty] string msg;
        private LoginResponse loginResponse = new LoginResponse();

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            return await _authService.LoginAsync(username, password);
        }
        /// <summary>
        /// Attempts to log in the user.
        /// </summary>
        [RelayCommand]
        public async Task Login()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Msg = string.Empty;
                loginResponse = await _authService.LoginAsync(Username, Password);
                //msg = loginResponse.
                if (loginResponse != null)
                {
                    
                    // save token to secure storage
                    await SecureStorage.Default.SetAsync("token", loginResponse.Token);

                    // save user id to secure storage
                    await SecureStorage.Default.SetAsync("userId", loginResponse.UserId.ToString());
                    string userid = await SecureStorage.Default.GetAsync("userId");
                    int.TryParse(userid, out int id);
                    PublicVariables._CurrentUserId = id;
                    _authService.IsUserLoggedIn = true;
                    // تعيين AppShell كصفحة رئيسية
                    if (Shell.Current == null)
                    {
                       
                        Application.Current.MainPage = new AppShell(_authService);
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//MapPage");
                    }

                    var boxes = await _box.GetBoxAsync();
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

                            }
                            break;
                        }
                    }
                    if (!isopen)
                    {
                        await Shell.Current.GoToAsync(nameof(OpenBoxPage), true);
                    }
                }
                else
                {
                    // Show an error message
                    await Application.Current.MainPage.DisplayAlert("Error", "Login failed. Please check your credentials.", "OK");
                }
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Toggles the password visibility.
        /// </summary>
        [RelayCommand]
        public void TogglePasswordVisibility()
        {
            this.IsPasswordVisible = !this.IsPasswordVisible;
        }
    }
}
