using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Services;

/// <summary>
/// Service responsible for showing notifications via system tray.
/// </summary>
/// <remarks>
/// Initializes a new instance of the NotificationService class.
/// </remarks>
/// <param name="notifyIcon">The NotifyIcon to use for notifications.</param>
public class NotificationService(NotifyIcon notifyIcon) : INotificationService
{
    private readonly NotifyIcon _notifyIcon = notifyIcon ?? throw new ArgumentNullException(nameof(notifyIcon));

    /// <summary>
    /// Shows an information notification.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    public void ShowInfo(string title, string message, int timeout = 3000)
    {
        ShowNotification(title, message, ToolTipIcon.Info, timeout);
    }

    /// <summary>
    /// Shows a warning notification.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    public void ShowWarning(string title, string message, int timeout = 3000)
    {
        ShowNotification(title, message, ToolTipIcon.Warning, timeout);
    }

    /// <summary>
    /// Shows an error notification.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    public void ShowError(string title, string message, int timeout = 3000)
    {
        ShowNotification(title, message, ToolTipIcon.Error, timeout);
    }

    /// <summary>
    /// Shows a notification with the specified parameters.
    /// </summary>
    /// <param name="title">The notification title.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="icon">The notification icon.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    private void ShowNotification(string title, string message, ToolTipIcon icon, int timeout)
    {
        _notifyIcon.BalloonTipTitle = title;
        _notifyIcon.BalloonTipText = message;
        _notifyIcon.BalloonTipIcon = icon;
        _notifyIcon.ShowBalloonTip(timeout);
    }
}