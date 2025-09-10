using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Flow.Launcher.Plugin.Caffeine.Tray;

/// <summary>
/// Factory class for creating NotifyIcon instances.
/// </summary>
internal static class TrayIconFactory
{
    /// <summary>
    /// Creates a new caffeine NotifyIcon instance.
    /// </summary>
    /// <param name="context">The plugin initialization context</param>
    /// <returns>The configured NotifyIcon instance</returns>
    public static NotifyIcon CreateCaffeineIcon(PluginInitContext context)
    {
        var notifyIcon = new NotifyIcon();
        var contextMenu = new ContextMenuStrip();
        
        var iconPath = Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "Images/icon.ico");
        if (File.Exists(iconPath))
            notifyIcon.Icon = new Icon(iconPath);

        notifyIcon.Text = "Caffeine is running.";
        notifyIcon.ContextMenuStrip = contextMenu;
        notifyIcon.Visible = false;

        return notifyIcon;
    }
}
