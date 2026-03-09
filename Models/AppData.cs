namespace HttpPostGet.Models;

public class AppData
{
    public List<SavedRequest> Favorites { get; set; } = [];
    public List<HistoryEntry> History { get; set; } = [];
    public bool IsDarkTheme { get; set; } = true;
}
