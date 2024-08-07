using GameCenterV2.Models;
using GameCenterV2.Services;
using Newtonsoft.Json;
using System.Text;
namespace GameCenterV2.Services
{
    public class AuthService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        public AuthService() : base()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is logged in.
        /// </summary>
        /// <remarks>
        /// Checks if the token is stored in secure storage. If set to false, removes the token and userId from secure storage.
        /// </remarks>
        public bool IsUserLoggedIn
        {
            get
            {
                var token = SecureStorage.GetAsync("token").Result;
                return !string.IsNullOrEmpty(token);
            }

            set
            {
                if (!value)
                {
                    // remove token and userId from secure storage if IsUserLoggedIn is set to false
                    SecureStorage.Remove("token");
                    SecureStorage.Remove("userId");
                }
            }
        }
       
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var loginRequest = new LoginRequest { Username = username, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                // Use the token for authentication
               // _authService.Token = loginResponse.Token;
                return loginResponse;
            }
            else
            {
                
            }

            return null;
        }
       
    }
}
