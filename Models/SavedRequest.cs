namespace HttpPoster.Models;

public class SavedRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = "POST";
    public string ContentType { get; set; } = "application/json";
    public string Body { get; set; } = string.Empty;
    public List<HttpHeader> Headers { get; set; } = [];
}
