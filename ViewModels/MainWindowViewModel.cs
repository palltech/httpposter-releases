using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HttpPostGet.Services;

namespace HttpPostGet.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DataService _dataService;

    public RequestViewModel Request { get; }
    public SidePanelViewModel SidePanel { get; }

    [ObservableProperty] private bool _isDarkTheme;
    public string ThemeIcon => IsDarkTheme ? "☀" : "☾";

    public MainWindowViewModel()
    {
        _dataService = new DataService();
        _dataService.Load();

        var httpService = new HttpService();
        Request = new RequestViewModel(httpService, _dataService);
        SidePanel = new SidePanelViewModel(_dataService);

        SidePanel.FavoriteSelected += r => Request.LoadFrom(r);
        SidePanel.HistorySelected += e => Request.LoadFromHistory(e);
        Request.HistoryEntryAdded += SidePanel.PrependHistoryEntry;

        SidePanel.Reload();

        IsDarkTheme = _dataService.Data.IsDarkTheme;
        ApplyTheme();
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        OnPropertyChanged(nameof(ThemeIcon));
        _dataService.Data.IsDarkTheme = value;
        _dataService.Save();
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        if (Avalonia.Application.Current is { } app)
            app.RequestedThemeVariant = IsDarkTheme ? ThemeVariant.Dark : ThemeVariant.Light;
    }

    [RelayCommand]
    private void ToggleTheme() => IsDarkTheme = !IsDarkTheme;

    [RelayCommand]
    private void SaveFavorite()
    {
        SidePanel.ShowSaveFavorite();
    }

    [RelayCommand]
    private void ConfirmSaveFavorite()
    {
        var request = Request.ToSavedRequest(string.Empty);
        SidePanel.ConfirmSaveFavorite(request);
    }
}
