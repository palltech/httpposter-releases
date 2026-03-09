using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HttpPostGet.Models;
using HttpPostGet.Services;
using System.Collections.ObjectModel;

namespace HttpPostGet.ViewModels;

public partial class RequestViewModel : ViewModelBase
{
    private readonly HttpService _httpService;
    private readonly DataService _dataService;

    public static IReadOnlyList<string> Methods { get; } = ["GET", "POST", "PUT", "DELETE", "PATCH"];

    public static IReadOnlyList<string> ContentTypes { get; } =
    [
        "text/xml",
        "application/json",
        "application/xml",
        "application/vnd.cip4-jdf+xml",
        "application/vnd.cip4-jmf+xml",
        "application/x-www-form-urlencoded",
        "text/plain",
        "text/html",
        "multipart/form-data"
    ];

    [ObservableProperty] private string _url = string.Empty;
    [ObservableProperty] private string _selectedMethod = "POST";
    [ObservableProperty] private string _selectedContentType = "text/xml";
    [ObservableProperty] private string _body = string.Empty;
    [ObservableProperty] private bool _isSending;
    [ObservableProperty] private bool _headersExpanded;

    public ObservableCollection<HeaderViewModel> Headers { get; } = [];
    public ResponseViewModel Response { get; } = new();

    public event Action<HistoryEntry>? HistoryEntryAdded;

    public RequestViewModel(HttpService httpService, DataService dataService)
    {
        _httpService = httpService;
        _dataService = dataService;
    }

    [RelayCommand]
    private void AddHeader()
    {
        var vm = new HeaderViewModel();
        vm.RemoveRequested += h => Headers.Remove(h);
        Headers.Add(vm);
        HeadersExpanded = true;
    }

    [RelayCommand]
    private async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(Url)) return;

        IsSending = true;
        Response.Clear();

        try
        {
            var result = await _httpService.SendAsync(
                Url, SelectedMethod, SelectedContentType, Body,
                Headers.Select(h => h.ToModel()));

            Response.SetResult(result);

            var entry = new HistoryEntry
            {
                Url = Url,
                Method = SelectedMethod,
                ContentType = SelectedContentType,
                Body = Body,
                Headers = Headers.Select(h => h.ToModel()).ToList(),
                ResponseStatusCode = result.ErrorMessage is null ? result.StatusCode : null
            };
            _dataService.AddToHistory(entry);
            HistoryEntryAdded?.Invoke(entry);
        }
        finally
        {
            IsSending = false;
        }
    }

    public void LoadFrom(SavedRequest request)
    {
        Url = request.Url;
        SelectedMethod = request.Method;
        SelectedContentType = request.ContentType;
        Body = request.Body;

        Headers.Clear();
        foreach (var h in request.Headers)
        {
            var vm = HeaderViewModel.FromModel(h);
            vm.RemoveRequested += header => Headers.Remove(header);
            Headers.Add(vm);
        }
    }

    public void LoadFromHistory(HistoryEntry entry)
    {
        Url = entry.Url;
        SelectedMethod = entry.Method;
        SelectedContentType = entry.ContentType;
        Body = entry.Body;

        Headers.Clear();
        foreach (var h in entry.Headers)
        {
            var vm = HeaderViewModel.FromModel(h);
            vm.RemoveRequested += header => Headers.Remove(header);
            Headers.Add(vm);
        }
    }

    public SavedRequest ToSavedRequest(string name) => new()
    {
        Name = name,
        Url = Url,
        Method = SelectedMethod,
        ContentType = SelectedContentType,
        Body = Body,
        Headers = Headers.Select(h => h.ToModel()).ToList()
    };
}
