using GameCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameCenterV2.Services
{
    public class ItemServices : BaseService
    {
        public async Task<IEnumerable<TbItem?>> GetItemAsync(string itemOrder = "asc")
        {
            return await GetAsync<IEnumerable<TbItem?>>($"/api/TbItem/sort?sortOrder={itemOrder}");
        }
        public async Task<TbItem?> GetItemByIdAsync(int id)
        {
            return await GetAsync<TbItem?>($"/api/TbItem/{id}");
        }
        public async Task<int> GetMealCountAsync()
        {
            try
            {
                var items = await GetItemAsync();
                return items.Count(item => item != null);
            }
            catch (Exception ex)
            {
                // يمكنك تسجيل الخطأ هنا إذا كنت ترغب في ذلك
                Console.WriteLine($"Error in GetMealCountAsync: {ex.Message}");
                return 0; // إرجاع 0 في حالة حدوث خطأ
            }
        }

        public async Task<bool> AddItemAsync(TbItem tbItem)
        {
            if (tbItem != null)
            {
                var json = JsonSerializer.Serialize(tbItem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/TbItem", content);

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
            else
            {
                return false;
            }
        }
        public async Task<bool> UpdateItem(TbItem tbItem)
        {
            if (tbItem != null)
            {
                try
                {
                    var json = JsonSerializer.Serialize(tbItem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"/api/TbItem/{tbItem.IId}", content);

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
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/TbItem/{id}");
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
