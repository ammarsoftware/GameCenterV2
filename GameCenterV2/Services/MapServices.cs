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
    public class MapServices : BaseService
    {
        public async Task<IEnumerable<TbTable?>> GetTableAsync(string sortOrder = "asc")
        {
            return await GetAsync<IEnumerable<TbTable?>>($"/api/TbTable/sort?sortOrder={sortOrder}");
        }

        public async Task<IEnumerable<TbMenu?>> GetActiveTableAsync()
        {
            return await GetAsync<IEnumerable<TbMenu?>>($"/api/TbTable/ActivTable");
        }
        public async Task<IEnumerable<TbTable?>> GetNotActiveTableAsync()
        {
            return await GetAsync<IEnumerable<TbTable?>>($"/api/TbTable/InactiveTables");
        }
        public async Task<TbTable?> GetTableByIdAsync(int id)
        {
            return await GetAsync<TbTable?>($"/api/TbTable/{id}");
        }

       
        public async Task<bool> AddTableAsync(TbTable tbtable)
        {
            if (tbtable != null)
            {
                var json = JsonSerializer.Serialize(tbtable, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/TbTable", content);

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
        public async Task<bool> UpdateTable(TbTable tbtable)
        {
            if (tbtable != null)
            {
                try
                {
                    //tbtable.TableImageString = tbtable.TableImage != null ? Convert.ToBase64String(tbtable.tableImage) : null;
                    var json = JsonSerializer.Serialize(tbtable, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync("/api/TbTable", content);

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

        public async Task<bool> DeleteTable(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/TbTable/{id}");
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
