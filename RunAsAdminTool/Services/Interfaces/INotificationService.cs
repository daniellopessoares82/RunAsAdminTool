namespace RunAsAdminTool.Services.Interfaces;

/// <summary>
/// Interface for notification service that handles system tray notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Shows an information notification.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    void ShowInfo(string title, string message, int timeout = 3000);

    /// <summary>
    /// Shows a warning notification.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    void ShowWarning(string title, string message, int timeout = 3000);

    /// <summary>
    /// Shows an error notification.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    void ShowError(string title, string message, int timeout = 3000);
}