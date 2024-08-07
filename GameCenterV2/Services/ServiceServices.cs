using GameCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameCenterV2.Services
{
    public partial class ServiceServices : BaseService
    {
        public async Task<IEnumerable<TbService?>> GetServiceAsync(int? MenuId)
        {
            if (MenuId == null)
            {
                return Enumerable.Empty<TbService>();
            }

            var endpoint = $"/api/TbService/{MenuId}";
            return await GetAsync<IEnumerable<TbService>>(endpoint);
        }
        public async Task<TbService?> GetServiceByIdAsync(int id)
        {
            return await GetAsync<TbService?>($"/api/TbService/{id}");
        }
        public async Task<bool> AddOrderItemsAsync(List<TbService> orderItems)
        {

            var json = JsonSerializer.Serialize(orderItems, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // استخدم null بدلاً من JsonNamingPolicy.CamelCase
                PropertyNameCaseInsensitive = true
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/TbService/orderitems", content);
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

        public async Task<bool> AddServiceAsync(TbService tbService)
        {
            if (tbService != null)
            {
                var json = JsonSerializer.Serialize(tbService, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/TbService", content);

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
        public async Task<bool> UpdateService(TbService tbService)
        {
            if (tbService != null)
            {
                try
                {
                    var json = JsonSerializer.Serialize(tbService, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync("/api/TbService", content);
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

        public async Task<bool> UpdateMerageTable(TbService tbService)
        {
            try
            {
                var json = JsonSerializer.Serialize(tbService, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/TbService/MerageTo", content);

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
        public async Task<bool> MovedTable(TbService tbService)
        {
            try
            {
                var json = JsonSerializer.Serialize(tbService, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/TbService/Moved", content);

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
        public async Task<bool> CancelMerageTable(TbService tbService)
        {
            try
            {
                var json = JsonSerializer.Serialize(tbService, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/TbService/CancelMerageTo", content);

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
        public async Task<bool> MoveTable(TbService tbService)
        {
            try
            {
                var json = JsonSerializer.Serialize(tbService, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/TbService/Moved", content);

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

        public async Task<bool> DeleteService(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/TbService/{id}");
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
