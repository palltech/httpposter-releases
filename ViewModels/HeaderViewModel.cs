using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HttpPoster.Models;

namespace HttpPoster.ViewModels;

public partial class HeaderViewModel : ViewModelBase
{
    [ObservableProperty] private string _key = string.Empty;
    [ObservableProperty] private string _value = string.Empty;

    public event Action<HeaderViewModel>? RemoveRequested;

    [RelayCommand]
    private void Remove() => RemoveRequested?.Invoke(this);

    public HttpHeader ToModel() => new() { Key = Key, Value = Value };

    public static HeaderViewModel FromModel(HttpHeader h) => new() { Key = h.Key, Value = h.Value };
}
