using GameCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameCenterV2.Services
{
    public partial class PrinterApiService : BaseService
    {
        //Print From Api Server
        public async Task<bool> PrintApiAsync(List<TbService> listservice)
        {
            var json = JsonSerializer.Serialize(listservice, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // استخدم null بدلاً من JsonNamingPolicy.CamelCase
                PropertyNameCaseInsensitive = true
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Print/print", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return false;
            }
        }
    }
}
