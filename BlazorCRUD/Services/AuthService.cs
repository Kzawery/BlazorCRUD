using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BlazorCRUD.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return !string.IsNullOrEmpty(token);
        }
        public async Task SetAuthorizationHeaderAsync()
        {
            if (_jsRuntime is IJSInProcessRuntime)
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }
        public Task ClearAuthorizationHeaderAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return Task.CompletedTask;
        }
    }

}
