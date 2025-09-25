using RunAsAdminTool.Models;
using RunAsAdminTool.Services;
using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool;

/// <summary>
/// Main form for the Run As Admin Tool application.
/// Provides a system tray interface for launching applications with administrator privileges.
/// </summary>
public partial class MainForm : Form
{
    private readonly ServiceContainer _serviceContainer;
    private readonly IConfigurationService _configurationService;
    private readonly IApplicationLauncherService _launcherService;
    private readonly IIconService _iconService;
    private readonly INotificationService _notificationService;
    
    private List<ApplicationInfo> _applications = [];

    /// <summary>
    /// Initializes a new instance of the MainForm class.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();
        
        // Initialize service container and configure services
        _serviceContainer = new ServiceContainer();
        ServiceConfiguration.ConfigureServices(_serviceContainer, notifyIcon);
        
        // Get services from container
        _configurationService = _serviceContainer.GetService<IConfigurationService>();
        _launcherService = _serviceContainer.GetService<IApplicationLauncherService>();
        _iconService = _serviceContainer.GetService<IIconService>();
        _notificationService = _serviceContainer.GetService<INotificationService>();
        
        InitializeApplication();
    }

    /// <summary>
    /// Initializes the application by loading configuration, setting up the icon, and showing startup notification.
    /// </summary>
    private void InitializeApplication()
    {
        LoadApplicationsFromConfiguration();
        SetupCustomIcon();
        
        // Start minimized to system tray
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = false;
        
        _notificationService.ShowInfo(
            "Run As Admin Tool", 
            "Application is running in the system tray");
    }

    /// <summary>
    /// Loads applications from the configuration service and creates the context menu.
    /// </summary>
    private void LoadApplicationsFromConfiguration()
    {
        _applications = _configurationService.LoadApplications();
        CreateContextMenu();
    }

    /// <summary>
    /// Sets up the custom application icon.
    /// </summary>
    private void SetupCustomIcon()
    {
        var appIcon = _iconService.LoadApplicationIcon();
        notifyIcon.Icon = appIcon;
        Icon = appIcon;
    }

    /// <summary>
    /// Creates the context menu with loaded applications and system options.
    /// </summary>
    private void CreateContextMenu()
    {
        contextMenuStrip.Items.Clear();

        // Add menu items for each application
        foreach (var app in _applications)
        {
            var menuItem = new ToolStripMenuItem(app.DisplayName)
            {
                Tag = app.Path
            };
            menuItem.Click += ApplicationMenuItem_Click;
            contextMenuStrip.Items.Add(menuItem);
        }

        // Add separator and exit item
        if (_applications.Count > 0)
        {
            contextMenuStrip.Items.Add(new ToolStripSeparator());
        }

        var exitItem = new ToolStripMenuItem("E&xit");
        exitItem.Click += ExitMenuItem_Click;
        contextMenuStrip.Items.Add(exitItem);
    }

    /// <summary>
    /// Handles the click event for application menu items.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ApplicationMenuItem_Click(object? sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string applicationPath)
        {
            _launcherService.LaunchAsAdmin(applicationPath, menuItem.Text ?? "Unknown Application");
        }
    }

    /// <summary>
    /// Handles the form resize event to hide the form when minimized.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void MainForm_Resize(object? sender, EventArgs e)
    {
    if (WindowState == FormWindowState.Minimized)
    {
        Hide();
        ShowInTaskbar = false;
    }
    }

    /// <summary>
    /// Handles the double-click event on the notify icon to reload applications.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
    {
        LoadApplicationsFromConfiguration();
        
        _notificationService.ShowInfo(
            "Applications Reloaded", 
            $"{_applications.Count} applications loaded successfully");
    }

    /// <summary>
    /// Handles the click event for the exit menu item.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        ExitApplication();
    }

    /// <summary>
    /// Exits the application and cleans up resources.
    /// </summary>
    private void ExitApplication()
    {
        notifyIcon.Visible = false;
        _serviceContainer.DisposeServices();
        Application.Exit();
    }

    /// <summary>
    /// Handles the form closing event to minimize to tray instead of closing.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
            Hide();
            ShowInTaskbar = false;

            _notificationService.ShowInfo(
                "Run As Admin Tool", 
                "Application minimized to tray. Double-click to reload applications.");
        }

        base.OnFormClosing(e);
    }

    /// <summary>
    /// Disposes of the form and its resources.
    /// </summary>
    /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _serviceContainer?.DisposeServices();
            
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }
            
            components?.Dispose();
        }
        base.Dispose(disposing);
    }
}
