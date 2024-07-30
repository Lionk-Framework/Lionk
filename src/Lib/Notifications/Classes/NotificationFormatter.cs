// Copyright © 2024 Lionk Project

using System.Text.Json;
using Serilog.Events;
using Serilog.Formatting;

/// <summary>
/// This class is used to format the notification log.
/// </summary>
public class NotificationFormatter : ITextFormatter
{
    /// <summary>
    /// Format the log event.
    /// </summary>
    /// <param name="logEvent"> The log event to format.</param>
    /// <param name="output"> The output to write the formatted log event to.</param>
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var logObject = new
        {
            logEvent.Timestamp,
            Level = logEvent.Level.ToString(),
            Message = logEvent.MessageTemplate,
            Notification = logEvent.Properties.ContainsKey("Notification") ? logEvent.Properties["Notification"].ToString()
                : null,
        };

        output.WriteLine(JsonSerializer.Serialize(logObject));
    }
}
