namespace ASPNetMicroserviceTemplate.HttpClients
{
    // Test, should be removed from real project!
    public class RobohashClient(HttpClient http)
    {
        private readonly HttpClient _http = http;

        public async Task<byte[]> GetAvatarAsync(string avatarId)
        {
            var resp = await _http.GetAsync($"{Uri.EscapeDataString(avatarId)}", HttpCompletionOption.ResponseHeadersRead);

            resp.EnsureSuccessStatusCode();      // кинет исключение на 4xx/5xx

            return await resp.Content.ReadAsByteArrayAsync();
        }
    }
}