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
    public partial class MenuServices : BaseService
    {
        public async Task<IEnumerable<TbMenu?>> GetMenuAsync()
        {
            return await GetAsync<IEnumerable<TbMenu?>>($"/api/TbMenu");
        }
        public async Task<decimal> GetMonyInBoxAsync(int boxId)
        {
            var response = await _httpClient.GetAsync($"api/TbMenu/GetMony?boxId={boxId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return decimal.Parse(content);
        }
        public async Task<TbMenu?> GetMenuByIdAsync(int id)
        {
            return await GetAsync<TbMenu?>($"/api/TbMenu/{id}");
        }
        public async Task<int> AutoNumber()
        {
            int number = 1;
            var menuList = await GetAsync<List<TbMenu>>($"/api/TbMenu/AutoNumber").ConfigureAwait(false);
            if (menuList != null && menuList.Any())
            {
                number = menuList.Max(m => m.MId) + 1;
            }
            return number;
        }

        public async Task<bool> AddMenuAsync(TbMenu tbMenu)
        {
            if (tbMenu != null)
            {
                var json = JsonSerializer.Serialize(tbMenu, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/TbMenu", content);

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
        public async Task<bool> UpdateMenu(TbMenu tbMenu)
        {
            if (tbMenu != null)
            {
                try
                {
                    var json = JsonSerializer.Serialize(tbMenu, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"/api/TbMenu/{tbMenu.MId}", content);

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

        public async Task<bool> DeleteMenu(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/TbMenu/{id}");
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

