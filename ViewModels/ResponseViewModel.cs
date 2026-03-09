using CommunityToolkit.Mvvm.ComponentModel;
using HttpPostGet.Services;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace HttpPostGet.ViewModels;

public partial class ResponseViewModel : ViewModelBase
{
    [ObservableProperty] private string _statusText = string.Empty;
    [ObservableProperty] private string _body = string.Empty;
    [ObservableProperty] private string _headersText = string.Empty;
    [ObservableProperty] private bool _hasResponse;
    [ObservableProperty] private bool _isError;

    public void SetResult(HttpResponseResult result)
    {
        if (result.ErrorMessage is not null)
        {
            IsError = true;
            StatusText = "Error";
            Body = result.ErrorMessage;
            HeadersText = string.Empty;
        }
        else
        {
            IsError = false;
            StatusText = $"{result.StatusCode} {result.ReasonPhrase}  ({result.Elapsed.TotalMilliseconds:0} ms)";
            Body = TryPrettyJson(result.Body) ?? TryPrettyXml(result.Body) ?? result.Body;
            HeadersText = string.Join("\n", result.Headers.Select(h => $"{h.Key}: {h.Value}"));
        }

        HasResponse = true;
    }

    public void Clear()
    {
        StatusText = string.Empty;
        Body = string.Empty;
        HeadersText = string.Empty;
        HasResponse = false;
        IsError = false;
    }

    private static string? TryPrettyJson(string body)
    {
        try
        {
            var doc = JsonDocument.Parse(body);
            return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch { return null; }
    }

    private static string? TryPrettyXml(string body)
    {
        try
        {
            var doc = XDocument.Parse(body);
            return doc.ToString(SaveOptions.None);
        }
        catch { return null; }
    }
}
