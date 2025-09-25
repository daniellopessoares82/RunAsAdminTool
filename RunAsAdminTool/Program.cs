namespace RunAsAdminTool;

/// <summary>
/// Main entry point for the Run As Admin Tool application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Configure application settings for high DPI and modern visual styles
        ApplicationConfiguration.Initialize();
        
        // Start the main form
        Application.Run(new MainForm());
    }
}