namespace Flow.Launcher.Plugin.Caffeine.Tray;

/// <summary>
/// Manages the system tray icon for the Caffeine plugin.
/// </summary>
public static class TrayIconManager
{
    private static TrayIconThreadManager _threadManager;
    private static readonly object _lock = new();
    private static volatile bool _isVisible = false;

    /// <summary>
    /// Shows the caffeine tray icon in a separate background thread. 
    /// This is necessary because the NotifyIcon gets disposed after 30 seconds
    /// when created/shown within the Caffeine.Init (Main.cs) method thread.
    /// </summary>
    /// <param name="context">The plugin initialization context</param>
    public static void ShowTray(PluginInitContext context)
    {
        lock (_lock)
        {
            if (_isVisible) return;

            _isVisible = true;
            _threadManager = new TrayIconThreadManager();
            
            var notifyIcon = TrayIconFactory.CreateCaffeineIcon(context);
            _threadManager.StartTrayThread(notifyIcon);
        }
    }

    /// <summary>
    /// Hides the caffeine tray icon and stops the thread.
    /// </summary>
    public static void HideTray()
    {
        lock (_lock)
        {
            if (!_isVisible) return;

            _isVisible = false;
            _threadManager?.Dispose();
            _threadManager = null;
        }
    }
}
