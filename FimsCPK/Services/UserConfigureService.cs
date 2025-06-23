using FimsCPK.Common;
using FimsCPK.Data;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FimsCPK.Services
{ 
    public class UserConfigureService
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UserConfigures");
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserConfigureService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            // 설정 파일을 저장할 디렉토리가 없으면 생성
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserConfiguration> LoadUserConfigAsync(string userId)
        {
            var filePath = Path.Combine(_storagePath, $"{userId}.json");
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<UserConfiguration>(json);
            }
            return null; // 파일이 없으면 null 반환
        }

        public async Task SaveUserConfigAsync(UserConfiguration config)
        {
            var filePath = Path.Combine(_storagePath, $"{config.UserId}.json");
            var json = JsonSerializer.Serialize(config);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task SaveUserConfigUsingDB(UserConfiguration request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiEndPoints.SaveUserConfig, request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to save grid state: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving grid state: {ex.Message}");
            }

        }

        public async Task<UserConfiguration> LoadUserConfigUsingDB(string GridName, string LoginId)
        {
            try
            {
                var absoluteUri = $"{ApiEndPoints.LoadUserConfig}?configName={GridName}&userId={LoginId}";
                UserConfiguration response = await _httpClient.GetFromJsonAsync<UserConfiguration>(absoluteUri);

                return response;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 저장된 상태가 없는 경우 - 기본 설정 사용
                Console.WriteLine("No saved grid state found. Using default settings.");
            }

            return null;
        }

        //--- 25.6.23
        public List<string> GetCurrentUserRoles()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                // ClaimTypes.Role 또는 "role"로 가져올 수 있습니다
                return user.Claims
                    .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();
            }

            return new List<string>();
        }


    }
}
