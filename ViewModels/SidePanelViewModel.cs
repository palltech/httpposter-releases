using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HttpPostGet.Models;
using HttpPostGet.Services;
using System.Collections.ObjectModel;

namespace HttpPostGet.ViewModels;

public partial class SidePanelViewModel : ViewModelBase
{
    private readonly DataService _dataService;

    [ObservableProperty] private string _newFavoriteName = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(IsNormalActionsVisible))] private bool _isSaveFavoriteVisible;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(IsNormalActionsVisible))] private bool _isRenameFavoriteVisible;
    [ObservableProperty] private string _renameFavoriteName = string.Empty;
    [ObservableProperty] private SavedRequest? _selectedFavorite;

    private SavedRequest? _favoriteToRename;

    public bool IsNormalActionsVisible => !IsSaveFavoriteVisible && !IsRenameFavoriteVisible;
    [ObservableProperty] private HistoryEntry? _selectedHistoryEntry;
    [ObservableProperty] private bool _isSaveHistoryAsFavoriteVisible;
    [ObservableProperty] private string _saveHistoryAsFavoriteName = string.Empty;

    private HistoryEntry? _historyEntryToSave;

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
        if (value is null) return;
        FavoriteSelected?.Invoke(value);
        SelectedFavorite = null;
    }

    partial void OnSelectedHistoryEntryChanged(HistoryEntry? value)
    {
        if (value is null) return;
        HistorySelected?.Invoke(value);
        SelectedHistoryEntry = null;
    }

    public void PrependHistoryEntry(HistoryEntry entry)
    {
        History.Insert(0, entry);
        while (History.Count > 10)
            History.RemoveAt(History.Count - 1);
    }

    private void InsertSorted(SavedRequest request)
    {
        var index = Favorites.TakeWhile(f => string.Compare(f.Name, request.Name, StringComparison.CurrentCultureIgnoreCase) <= 0).Count();
        Favorites.Insert(index, request);
    }

    public void Reload()
    {
        Favorites.Clear();
        foreach (var f in _dataService.Data.Favorites.OrderBy(f => f.Name, StringComparer.CurrentCultureIgnoreCase))
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

    [RelayCommand]
    private void DeleteFavorite(SavedRequest request)
    {
        _dataService.RemoveFavorite(request.Id);
        Favorites.Remove(request);
    }

    [RelayCommand]
    private void StartRenameFavorite(SavedRequest request)
    {
        _favoriteToRename = request;
        RenameFavoriteName = request.Name;
        IsRenameFavoriteVisible = true;
        IsSaveFavoriteVisible = false;
    }

    [RelayCommand]
    private void ConfirmRenameFavorite()
    {
        if (_favoriteToRename is null || string.IsNullOrWhiteSpace(RenameFavoriteName)) return;

        _dataService.RenameFavorite(_favoriteToRename.Id, RenameFavoriteName);

        // Refresh item in the list (SavedRequest is plain class, no INPC)
        var idx = Favorites.IndexOf(_favoriteToRename);
        _favoriteToRename.Name = RenameFavoriteName;
        if (idx >= 0) { Favorites.RemoveAt(idx); InsertSorted(_favoriteToRename); }

        _favoriteToRename = null;
        RenameFavoriteName = string.Empty;
        IsRenameFavoriteVisible = false;
    }

    [RelayCommand]
    private void CancelRenameFavorite()
    {
        _favoriteToRename = null;
        RenameFavoriteName = string.Empty;
        IsRenameFavoriteVisible = false;
    }

    [RelayCommand]
    private void DeleteHistoryEntry(HistoryEntry entry)
    {
        _dataService.RemoveHistoryEntry(entry.Id);
        History.Remove(entry);
    }

    [RelayCommand]
    private void ClearHistory()
    {
        _dataService.ClearHistory();
        History.Clear();
    }

    [RelayCommand]
    private void StartSaveHistoryAsFavorite(HistoryEntry entry)
    {
        _historyEntryToSave = entry;
        SaveHistoryAsFavoriteName = $"Favorite {entry.SentAt:yyyy-MM-dd HH:mm}";
        IsSaveHistoryAsFavoriteVisible = true;
    }

    [RelayCommand]
    private void ConfirmSaveHistoryAsFavorite()
    {
        if (_historyEntryToSave is null || string.IsNullOrWhiteSpace(SaveHistoryAsFavoriteName)) return;

        var request = new SavedRequest
        {
            Name = SaveHistoryAsFavoriteName,
            Url = _historyEntryToSave.Url,
            Method = _historyEntryToSave.Method,
            ContentType = _historyEntryToSave.ContentType,
            Body = _historyEntryToSave.Body,
            Headers = _historyEntryToSave.Headers.ToList()
        };
        _dataService.AddFavorite(request);
        InsertSorted(request);

        _historyEntryToSave = null;
        SaveHistoryAsFavoriteName = string.Empty;
        IsSaveHistoryAsFavoriteVisible = false;
    }

    [RelayCommand]
    private void CancelSaveHistoryAsFavorite()
    {
        _historyEntryToSave = null;
        SaveHistoryAsFavoriteName = string.Empty;
        IsSaveHistoryAsFavoriteVisible = false;
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
        InsertSorted(request);

        NewFavoriteName = string.Empty;
        IsSaveFavoriteVisible = false;
        return true;
    }
}
