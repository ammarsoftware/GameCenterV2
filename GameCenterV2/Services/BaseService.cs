using GameCenterV2.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace GameCenterV2.Services
{
    public class BaseService
    {
        /// <summary>
        /// An instance of <see cref="HttpClient"/>.
        /// </summary>
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        public BaseService()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            handler.AllowAutoRedirect = false;
            //{
            //    AllowAutoRedirect = false, // تعطيل إعادة التوجيه التلقائي
            //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
            //};
            string ipAddress = Preferences.Get("DatabaseIpAddress", "default_value");
            
            //handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler)
            {
                
                BaseAddress = new Uri($"https://{ipAddress}:5244/"),
            };
        }

        /// <summary>
        /// Sends a GET request to the specified endpoint and returns the response as an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="endpoint">The endpoint to send the GET request to.</param>
        /// <returns>A task of type <typeparamref name="T"/>.</returns>
        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            if (!IsInternetAvailable())
            {
                return default;
            }

            try
            {
                var response = await _httpClient.GetAsync(endpoint).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                // استخدام System.Text.Json لتحويل البيانات
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // جعل التسميات غير حساسة لحالة الأحرف
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters =
                        {
                            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                        }

                };
                return System.Text.Json.JsonSerializer.Deserialize<T>(responseContent,options);
                
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", "Unable to get data.", "OK");
                return default;
            }
        }

        /// <summary>
        /// Sends a DELETE request to the specified endpoint and returns the response.
        /// </summary>
        /// <param name="endpoint">The endpoint to send the DELETE request to.</param>
        /// <returns>A task of type <see cref="HttpResponseMessage"/>.</returns>
        protected async Task<HttpResponseMessage?> DeleteAsync(string endpoint)
        {
            if (!IsInternetAvailable())
            {
                return null;
            }

            try
            {
                return await _httpClient.DeleteAsync(endpoint);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", "Unable to delete data.", "OK");
                return null;
            }
        }

        /// <summary>
        /// Checks if an internet connection is available.
        /// </summary>
        /// <returns><c>true</c> if an internet connection is available; otherwise, <c>false</c>.</returns>
        private bool IsInternetAvailable()
        {
            NetworkAccess accessType = Connectivity.NetworkAccess;

            if (accessType != NetworkAccess.Internet)
            {
                if (Shell.Current != null)
                {
                    if (accessType == NetworkAccess.ConstrainedInternet)
                    {
                        Shell.Current.DisplayAlert("Error!", "Internet access is limited.", "OK");
                    }
                    else if(accessType == NetworkAccess.Local)
                    {
                        return true;
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Error!", "No internet access.", "OK");
                    }
                }

                return false;
            }

            return true;
        }
    }
}
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}