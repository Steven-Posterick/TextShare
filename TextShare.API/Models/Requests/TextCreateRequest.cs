namespace TextShare.API.Models.Requests;

public class TextCreateRequest
{
    public string Content { get; set; }
    public string? Password { get; set; }
    public bool BurnAfterReading { get; set; }
}