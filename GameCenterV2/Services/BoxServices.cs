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
    public class BoxServices : BaseService
    {
        public async Task<IEnumerable<TbBox?>> GetBoxAsync()
        {
            return await GetAsync<IEnumerable<TbBox?>>($"/api/TbBox");
        }
        public async Task<TbBox?> GetBoxByIdAsync(int id)
        {
            return await GetAsync<TbBox?>($"/api/TbBox/{id}");
        }


        public async Task<bool> AddBoxAsync(TbBox tbBox)
        {
            if (tbBox != null)
            {
                var json = JsonSerializer.Serialize(tbBox, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/TbBox", content);

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
        public async Task<bool> UpdateBox(TbBox tbBox)
        {
            if (tbBox != null)
            {
                try
                {
                    //tbBox.BoxImageString = tbBox.BoxImage != null ? Convert.ToBase64String(tbBox.BoxImage) : null;
                    var json = JsonSerializer.Serialize(tbBox, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    
                    var response = await _httpClient.PutAsync($"/api/TbBox/{tbBox.BoxId}", content);

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

        public async Task<bool> DeleteBox(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/TbBox/{id}");
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
