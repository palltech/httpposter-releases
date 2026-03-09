namespace HttpPoster.Models;

public class AddressBookEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DisplayName => string.IsNullOrWhiteSpace(Description) ? Url : Description;
}
