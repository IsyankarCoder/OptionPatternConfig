using Microsoft.Extensions.Options;
using OptionPatternConfig.ApiSettings;

namespace OptionPatternConfig.ApiSettings;

public interface IApiSettingsForOptionClient
{
    Task<string> Execute(CancellationToken cancellationToken = default);
}


public class ApiSettingsForOptionClient(HttpClient httpClient, IOptions<ApiSettingsForOption> options)
    : IApiSettingsForOptionClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettingsForOption _apiSettings = options.Value;

    public async Task<string> Execute(CancellationToken cancellationToken = default)
    {
        /*var requestUri = $"{_apiSettings.BaseUrl}{_apiSettings.Version}/data?apiKey={_apiSettings.ApiKey}";
        var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();*/

        HttpRequestMessage request = new(HttpMethod.Get, _apiSettings.BaseUrl);
        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}