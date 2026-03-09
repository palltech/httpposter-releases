using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HttpPoster.Models;
using HttpPoster.Services;
using System.Collections.ObjectModel;

namespace HttpPoster.ViewModels;

public partial class SidePanelViewModel : ViewModelBase
{
    private readonly DataService _dataService;

    [ObservableProperty] private string _newFavoriteName = string.Empty;
    [ObservableProperty] private bool _isSaveFavoriteVisible;
    [ObservableProperty] private SavedRequest? _selectedFavorite;
    [ObservableProperty] private HistoryEntry? _selectedHistoryEntry;

    public ObservableCollection<SavedRequest> Favorites { get; } = [];
    public ObservableCollection<HistoryEntry> History { get; } = [];

    public event Action<SavedRequest>? FavoriteSelected;
    public event Action<HistoryEntry>? HistorySelected;

    public SidePanelViewModel(DataService dataService)
    {
        _dataService = dataService;
    }

    partial void OnSelectedFavoriteChanged(SavedRequest? value)
    {
        if (value is not null) FavoriteSelected?.Invoke(value);
    }

    partial void OnSelectedHistoryEntryChanged(HistoryEntry? value)
    {
        if (value is not null) HistorySelected?.Invoke(value);
    }

    public void Reload()
    {
        Favorites.Clear();
        foreach (var f in _dataService.Data.Favorites)
            Favorites.Add(f);

        History.Clear();
        foreach (var h in _dataService.Data.History)
            History.Add(h);
    }

    [RelayCommand]
    private void RemoveSelectedFavorite()
    {
        if (SelectedFavorite is null) return;
        _dataService.RemoveFavorite(SelectedFavorite.Id);
        Favorites.Remove(SelectedFavorite);
        SelectedFavorite = null;
    }

    public void ShowSaveFavorite() => IsSaveFavoriteVisible = true;

    [RelayCommand]
    private void CancelSaveFavorite()
    {
        NewFavoriteName = string.Empty;
        IsSaveFavoriteVisible = false;
    }

    public bool ConfirmSaveFavorite(SavedRequest request)
    {
        if (string.IsNullOrWhiteSpace(NewFavoriteName)) return false;

        request.Name = NewFavoriteName;
        _dataService.AddFavorite(request);
        Favorites.Add(request);

        NewFavoriteName = string.Empty;
        IsSaveFavoriteVisible = false;
        return true;
    }
}
