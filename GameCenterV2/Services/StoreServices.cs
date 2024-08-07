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
    public class StoreServices : BaseService
    {
        public async Task<IEnumerable<TbStore?>> GetStoreAsync(string sortOrder = "asc")
        {
            return await GetAsync<IEnumerable<TbStore?>>($"/api/TbStore/sort?sortOrder={sortOrder}");
        }
        public async Task<TbStore?> GetStoreByIdAsync(int id)
        {
            return await GetAsync<TbStore?>($"/api/TbStore/{id}");
        }


        public async Task<bool> AddStoreAsync(TbStore tbStore)
        {
            if (tbStore != null)
            {
                var json = JsonSerializer.Serialize(tbStore, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/TbStore", content);

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
        public async Task<bool> UpdateStore(TbStore tbStore)
        {
            if (tbStore != null)
            {
                try
                {
                    //tbStore.StoreImageString = tbStore.StoreImage != null ? Convert.ToBase64String(tbStore.StoreImage) : null;
                    var json = JsonSerializer.Serialize(tbStore, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"/api/TbStore/{tbStore.StoreId}", content);

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

        public async Task<bool> DeleteStore(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/TbStore/{id}");
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
