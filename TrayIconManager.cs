using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Flow.Launcher.Plugin.Caffeine;

/// <summary>
/// Manages the system tray icon for the Caffeine plugin.
/// </summary>
public static class TrayIconManager
{
    private static NotifyIcon _notifyIcon;
    private static Thread _trayThread;
    private static readonly object _lock = new();
    private static volatile bool _isVisible = false;

    /// <summary>
    /// Shows the caffeine tray icon in a separate background thread. 
    /// This is necessary because the NotifyIcon gets disposed after 30 seconds
    /// when created/shown within the Caffeine.Init (Main.cs) method thread.
    /// </summary>
    public static void ShowTray()
    {
        lock (_lock)
        {
            if (_isVisible) return;

            _isVisible = true;
            
            // Create and start a new STA thread for the tray icon
            _trayThread = new Thread(() =>
            {
                _notifyIcon = GenerateNotifyIcon();
                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = true;
                    
                    // Run the message pump to keep the tray icon alive
                    Application.Run();
                }
            })
            {
                IsBackground = true, // Allow the application to exit
                Name = "CaffeineTrayIconThread"
            };
            
            _trayThread.SetApartmentState(ApartmentState.STA);
            _trayThread.Start();
        }
    }

    /// <summary>
    /// Hides the caffeine tray icon and stops the thread
    /// </summary>
    public static void HideTray()
    {
        lock (_lock)
        {
            if (!_isVisible) return;

            _isVisible = false;

            if (_notifyIcon != null)
            {
                try
                {
                    // Try to invoke on the tray thread if possible
                    if (_notifyIcon.ContextMenuStrip != null)
                    {
                        _notifyIcon.ContextMenuStrip.Invoke(new System.Action(() =>
                        {
                            _notifyIcon.Visible = false;
                            _notifyIcon.Dispose();
                            _notifyIcon = null;
                            Application.ExitThread();
                        }));
                    }
                    else
                    {
                        // Direct cleanup if no context menu available
                        _notifyIcon.Visible = false;
                        _notifyIcon.Dispose();
                        _notifyIcon = null;
                    }
                }
                catch
                {
                    // Force cleanup if invoke fails
                    try
                    {
                        _notifyIcon.Visible = false;
                        _notifyIcon.Dispose();
                        _notifyIcon = null;
                    }
                    catch { }
                }
            }

            // Wait for thread to finish (with 200ms timeout)
            _trayThread?.Join(200);
            _trayThread = null;
        }
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
