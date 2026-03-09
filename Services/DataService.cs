using System.Text.Json;
using HttpPostGet.Models;

namespace HttpPostGet.Services;

public class DataService
{
    private static readonly string DataFilePath = Path.Combine(
        AppContext.BaseDirectory, "httpposter.data.json");

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

    public void RemoveHistoryEntry(Guid id)
    {
        _data.History.RemoveAll(h => h.Id == id);
        Save();
    }

    public void ClearHistory()
    {
        _data.History.Clear();
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

    public void RenameFavorite(Guid id, string newName)
    {
        var f = _data.Favorites.FirstOrDefault(f => f.Id == id);
        if (f is null) return;
        f.Name = newName;
        Save();
    }

}
