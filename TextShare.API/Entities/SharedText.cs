namespace TextShare.API.Entities;

public class SharedText
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool BurnAfterReading { get; set; }
}