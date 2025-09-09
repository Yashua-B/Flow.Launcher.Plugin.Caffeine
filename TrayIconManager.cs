using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Flow.Launcher.Plugin.Caffeine;

/// <summary>
/// Manages the system tray icon for the Caffeine plugin.
/// 
/// BUG: This class does not function properly if called within Caffeine Plugin Init method. 
/// The tray icon will be disposed 30 seconds after plugin initialization. 
/// I was not able to find a nice solution for this except for hide/show icon 
/// every 30 seconds which is not a good solution.
/// </summary>
public static class TrayIconManager
{

    private static NotifyIcon _notifyIcon;

    /// <summary>
    /// Shows the caffeine tray icon
    /// </summary>
    public static void ShowTray()
    {
        _notifyIcon = GenerateNotifyIcon();
        if (_notifyIcon != null)
        {
            _notifyIcon.Visible = true;
        }
    }

    /// <summary>
    /// Hides the caffeine tray icon
    /// </summary>
    public static void HideTray()
    {
        _notifyIcon?.Dispose();
        _notifyIcon = null;
    }

    /// <summary>
    /// Generates a new caffeine NotifyIcon instance
    /// </summary>
    /// <returns>The new NotifyIcon instance</returns>
    private static NotifyIcon GenerateNotifyIcon()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var pluginPath = Path.GetDirectoryName(currentAssembly.Location);

        var notifyIcon = new NotifyIcon();
        var contextMenu = new ContextMenuStrip();

        var iconPath = Path.Combine(pluginPath, "Images/icon.ico");
        if (File.Exists(iconPath))
            notifyIcon.Icon = new Icon(iconPath);

        notifyIcon.Text = "Caffeine is running.";
        notifyIcon.ContextMenuStrip = contextMenu;
        notifyIcon.Visible = false;

        return notifyIcon;
    }
}
