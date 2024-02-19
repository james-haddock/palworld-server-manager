using System.Net.Http;

public static class GetIpAddress
{
    public static async Task<string> GetPublicIpAddressAsync()
    {
        using var httpClient = new HttpClient();
        string publicIpAddress = await httpClient.GetStringAsync("https://api.ipify.org");
        return publicIpAddress;
    }
}
