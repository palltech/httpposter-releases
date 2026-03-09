using System.Net.Http.Headers;
using System.Text;
using HttpPoster.Models;

namespace HttpPoster.Services;

public class HttpResponseResult
{
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = [];
    public string Body { get; set; } = string.Empty;
    public TimeSpan Elapsed { get; set; }
    public string? ErrorMessage { get; set; }
}

public class HttpService
{
    private readonly HttpClient _client = new();

    public async Task<HttpResponseResult> SendAsync(
        string url,
        string method,
        string contentType,
        string body,
        IEnumerable<HttpHeader> headers)
    {
        var result = new HttpResponseResult();
        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var request = new HttpRequestMessage(new HttpMethod(method), url);

            foreach (var header in headers)
            {
                if (!string.IsNullOrWhiteSpace(header.Key))
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (method is not "GET" and not "HEAD" && !string.IsNullOrEmpty(body))
            {
                request.Content = new StringContent(body, Encoding.UTF8, contentType);
            }

            var response = await _client.SendAsync(request);
            sw.Stop();

            result.StatusCode = (int)response.StatusCode;
            result.ReasonPhrase = response.ReasonPhrase ?? string.Empty;
            result.Elapsed = sw.Elapsed;

            foreach (var header in response.Headers)
                result.Headers[header.Key] = string.Join(", ", header.Value);
            foreach (var header in response.Content.Headers)
                result.Headers[header.Key] = string.Join(", ", header.Value);

            result.Body = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            sw.Stop();
            result.Elapsed = sw.Elapsed;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}
