﻿namespace TextShare.API.Models.Responses;

public class TextDetailResponse
{
    public Guid Id { get; set; }
    public bool BurnAfterRead { get; set; }
    public bool IsPasswordProtected { get; set; }
}