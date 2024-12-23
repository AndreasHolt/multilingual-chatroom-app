using System;

namespace MultilingualChat.Client.Models;

public class Message
{
    public string MessageText { get; set; }

    public string MessageSenderColor { get; set; }
    public string MessageSender { get; set; }
    public DateTime MessageTimestamp { get; set; }
}