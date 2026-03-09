using System.Text.Json;
using HttpPoster.Models;

namespace HttpPoster.Services;

public class DataService
{
    private static readonly string DataFilePath = Path.Combine(
        AppContext.BaseDirectory, "data", "data.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private AppData _data = new();

    public AppData Data => _data;

    public void Load()
    {
        try
        {
            if (!File.Exists(DataFilePath))
            {
                _data = new AppData();
                return;
            }

            var json = File.ReadAllText(DataFilePath);
            _data = JsonSerializer.Deserialize<AppData>(json, JsonOptions) ?? new AppData();
        }
        catch
        {
            _data = new AppData();
        }
    }

    public void Save()
    {
        var dir = Path.GetDirectoryName(DataFilePath)!;
        Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(_data, JsonOptions);
        File.WriteAllText(DataFilePath, json);
    }

    public void AddToHistory(HistoryEntry entry)
    {
        _data.History.Insert(0, entry);
        if (_data.History.Count > 10)
            _data.History.RemoveRange(10, _data.History.Count - 10);
        Save();
    }

    public void AddFavorite(SavedRequest request)
    {
        _data.Favorites.Add(request);
        Save();
    }

    public void RemoveFavorite(Guid id)
    {
        _data.Favorites.RemoveAll(f => f.Id == id);
        Save();
    }

    public void AddAddressBookEntry(AddressBookEntry entry)
    {
        _data.AddressBook.Add(entry);
        Save();
    }

    public void RemoveAddressBookEntry(Guid id)
    {
        _data.AddressBook.RemoveAll(e => e.Id == id);
        Save();
    }
}
