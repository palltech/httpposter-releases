using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HttpPoster.Models;
using HttpPoster.Services;
using System.Collections.ObjectModel;

namespace HttpPoster.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DataService _dataService;

    public RequestViewModel Request { get; }
    public SidePanelViewModel SidePanel { get; }

    public ObservableCollection<AddressBookEntry> AddressBook { get; } = [];
    [ObservableProperty] private AddressBookEntry? _selectedAddressBookEntry;

    public MainWindowViewModel()
    {
        _dataService = new DataService();
        _dataService.Load();

        var httpService = new HttpService();
        Request = new RequestViewModel(httpService, _dataService);
        SidePanel = new SidePanelViewModel(_dataService);

        SidePanel.FavoriteSelected += r => Request.LoadFrom(r);
        SidePanel.HistorySelected += e => Request.LoadFromHistory(e);

        SidePanel.Reload();
        ReloadAddressBook();
    }

    partial void OnSelectedAddressBookEntryChanged(AddressBookEntry? value)
    {
        if (value is not null)
            Request.Url = value.Url;
    }

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

    private void ReloadAddressBook()
    {
        AddressBook.Clear();
        foreach (var e in _dataService.Data.AddressBook)
            AddressBook.Add(e);
    }

    [RelayCommand]
    private void AddAddressBookEntry(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;
        var entry = new AddressBookEntry { Url = url };
        _dataService.AddAddressBookEntry(entry);
        AddressBook.Add(entry);
    }

    [RelayCommand]
    private void RemoveAddressBookEntry(AddressBookEntry entry)
    {
        _dataService.RemoveAddressBookEntry(entry.Id);
        AddressBook.Remove(entry);
    }
}
