using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Flow.Launcher.Plugin;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Controls;
using Flow.Launcher.Plugin.Caffeine.Settings;

namespace Flow.Launcher.Plugin.Caffeine
{
    public static class TrayIconManager
    {
        private static NotifyIcon _notifyIcon;
        private static ContextMenuStrip _contextMenu;

        // Only way i know how to get the script folder.. If anyone knows how to get this within the plugin api please let me know
        private static Assembly currentAssembly = Assembly.GetExecutingAssembly();
        static string dllPath = currentAssembly.Location;
        static string pluginPath = Path.GetDirectoryName(dllPath);
        public static void Initialize()
        {
            _notifyIcon = new NotifyIcon();
            _contextMenu = new ContextMenuStrip();

            // Set the icon
            _notifyIcon.Icon = new Icon(Path.Combine(pluginPath, "Images/icon.ico"));

            // Set the tooltip text
            _notifyIcon.Text = "Caffeine is running.";

            // Make the icon hidden
            _notifyIcon.Visible = false;
        }

        public static void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }

        public static void ShowTray()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = true;
            }
        }

        public static void HideTray()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
            }
        }

        // Example method to show a form (you'd replace this with your actual form logic)
        private static void ShowApplicationForm()
        {
            // Example: If you have a main form, you'd show it here.
            // Form1 mainForm = new Form1();
            // mainForm.Show();
            MessageBox.Show("Application form would be shown here.", "Tray Icon Demo");
        }
    }
    public class Caffeine : IPlugin, ISettingProvider
    {
        internal PluginInitContext Context;
        private bool _enabled = false;
        private Settings.Settings _settings;
        internal static class NativeMethods
        {
            // Import SetThreadExecutionState Win32 API and necessary flags
            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);
            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;
            public const uint ES_AWAYMODE_REQUIRED = 0x00000040;
            public const uint ES_DISPLAY_REQUIRED = 0x00000040;
        }

        public List<Result> Query(Query query)
        {
            var result = new Result
            {
                Title = $"Turn {CaffeineState()} Caffeine",
                SubTitle = "Toggle Caffeine off and on.",
                Action = c =>
                {
                    if (!_enabled)
                    {
                        StartCaffeine();
                    }
                    else
                    {
                        StopCaffeine();
                    }
                    return true;
                },
                IcoPath = "Images/icon.png"
            };
            return new List<Result> { result };
        }

        public string CaffeineState()
        {
            if (!_enabled)
            {
                return "On";
            }
            else
            {
                return "Off";
            }
        }

        private void KeepAlive()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED | NativeMethods.ES_AWAYMODE_REQUIRED);
        }
        
        public void Init(PluginInitContext context)
        {
            Context = context;
            TrayIconManager.Initialize();
            _settings = context.API.LoadSettingJsonStorage<Settings.Settings>();
            
            // Start caffeine automatically if the setting is enabled
            if (_settings.StartWithFlowLauncher)
            {
                StartCaffeine();
            }
        }

        private void StartCaffeine()
        {
            if (!_enabled)
            {
                PowerUtilities.PreventPowerSave();
                TrayIconManager.ShowTray();
                _enabled = true;
            }
        }

        private void StopCaffeine()
        {
            if (_enabled)
            {
                PowerUtilities.Shutdown();
                TrayIconManager.HideTray();
                _enabled = false;
            }
        }

        public System.Windows.Controls.Control CreateSettingPanel()
        {
            return new PluginSettings(Context, _settings);
        }

    }
    public static class PowerUtilities
    {
        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);

        private static AutoResetEvent _event = new AutoResetEvent(false);

        public static void PreventPowerSave()
        {
            (new TaskFactory()).StartNew(() =>
                {
                    SetThreadExecutionState(
                        EXECUTION_STATE.ES_CONTINUOUS
                        | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                        | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                    _event.WaitOne();

                },
                TaskCreationOptions.LongRunning);
        }

        public static void Shutdown()
        {
            _event.Set();
        }
    }
}