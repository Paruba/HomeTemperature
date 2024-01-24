using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace Boiler.Mobile.Framework.Http;

public class ApiClient : HttpClient
{
    private const string ApplicationJsonMediaType = "application/json";
    private const string RequestIdHeader = "X-Api-RequestId";
    public event Action OnUnauthorizedResponse;

    public ApiClient(HttpMessageHandler handler) : base(handler)
    {
    }

    public new async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        var response = await base.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            OnUnauthorizedResponse?.Invoke();
        }
        return response;
    }

    public Task<TResult> PostFromJsonAsync<TResult>(string requestUri, object content = null)
    {
        return PostFromJsonAsync<TResult>(BaseAddress == null ? new Uri(requestUri) : new Uri(BaseAddress, requestUri), content, null);
    }

    public async Task<TResult> PostFromJsonAsync<TResult>(Uri requestUri, object content, JsonSerializerOptions options, CancellationToken cancellationToken = default)
    {
        string requestId = Guid.NewGuid().ToString();
        HttpResponseMessage response = null;

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Add(RequestIdHeader, requestId);
            request.Content = JsonContent.Create(content, new MediaTypeHeaderValue(ApplicationJsonMediaType), options ?? JsonSerializerDefaults.DefaultSettings);

            response = await SendAsync(request, cancellationToken);


            var obj = await DeserializeAsync<TResult>(response.Content, options, cancellationToken);
            return obj;
        }
        catch (HttpRequestException ex)
        {
            Console.Write(ex.Message);
            throw ex;
        }
    }

    public static async Task<T> DeserializeAsync<T>(HttpContent httpContent, JsonSerializerOptions jsonSerializerSettings, CancellationToken cancellationToken = default)
    {
        string json = await httpContent.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrEmpty(json))
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(json, jsonSerializerSettings ?? JsonSerializerDefaults.DefaultSettings);
    }
}