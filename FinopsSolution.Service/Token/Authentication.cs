using FinopsSolution.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Token
{
    internal class Authentication
    {
        private static HttpClient? _httpClient = null;
        private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private DateTime _expiresAt = DateTime.UtcNow;
        internal async Task<HttpClient> GenerateClient()
        {
            if (_httpClient == null || DateTime.UtcNow>_expiresAt)
            {
                 await _semaphoreSlim
                        .WaitAsync()
                        .ConfigureAwait(false);

                _httpClient = new HttpClient();
                _expiresAt = DateTime.UtcNow.AddSeconds(90);
                var token = await AzureIdentityService.GetToken(new(Utils.TenantId, Utils.ClientId, Utils.ClientSecret));
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                _semaphoreSlim.Release();
            }

            
            
            return _httpClient;
        }
    }
}
