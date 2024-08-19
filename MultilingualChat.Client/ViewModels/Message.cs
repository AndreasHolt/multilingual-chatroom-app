using System;

namespace MultilingualChat.Client.ViewModels;

public class Message
{
    public string MessageText { get; set; }
    public string MessageSender { get; set; }
    public DateTime MessageTimestamp { get; set; }
}