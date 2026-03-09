namespace HttpPostGet.Models;

public class HistoryEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime SentAt { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<HttpHeader> Headers { get; set; } = [];
    public int? ResponseStatusCode { get; set; }
    public string DisplayText => $"{Method}  {Url}";
}
