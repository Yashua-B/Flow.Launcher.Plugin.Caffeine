using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Flow.Launcher.Plugin.Caffeine
{
    /// <summary>
    /// Manages the system tray icon for the Caffeine plugin
    /// </summary>
    public static class TrayIconManager
    {
        private static NotifyIcon _notifyIcon;
        private static ContextMenuStrip _contextMenu;

        // Only way i know how to get the script folder.. If anyone knows how to get this within the plugin api please let me know
        private static Assembly currentAssembly = Assembly.GetExecutingAssembly();
        static string dllPath = currentAssembly.Location;
        static string pluginPath = Path.GetDirectoryName(dllPath);
        
        /// <summary>
        /// Initialize the tray icon
        /// </summary>
        public static void Initialize()
        {
            _notifyIcon = new NotifyIcon();
            _contextMenu = new ContextMenuStrip();

            // Set the icon
            _notifyIcon.Icon = new Icon(Path.Combine(pluginPath, "Images/icon.ico"));

            // Set the tooltip text
            _notifyIcon.Text = "Caffeine is running.";

            // Make the icon hidden initially
            _notifyIcon.Visible = false;
        }

        /// <summary>
        /// Dispose of the tray icon resources
        /// </summary>
        public static void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }

        /// <summary>
        /// Show the tray icon
        /// </summary>
        public static void ShowTray()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = true;
            }
        }

        /// <summary>
        /// Hide the tray icon
        /// </summary>
        public static void HideTray()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
            }
        }

        /// <summary>
        /// Check if the tray icon is initialized
        /// </summary>
        public static bool IsInitialized => _notifyIcon != null;
    }
}
