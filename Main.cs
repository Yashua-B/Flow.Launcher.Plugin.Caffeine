using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Flow.Launcher.Plugin;
using Flow.Launcher.Plugin.Caffeine.Settings;

namespace Flow.Launcher.Plugin.Caffeine
{
    /// <summary>
    /// Main plugin class for the Caffeine Flow Launcher plugin
    /// </summary>
    public class Caffeine : IPlugin, ISettingProvider
    {
        internal PluginInitContext Context;
        private bool _enabled = false;
        private Settings.Settings _settings;

        /// <summary>
        /// Query method for Flow Launcher
        /// </summary>
        /// <param name="query">The query from Flow Launcher</param>
        /// <returns>List of results</returns>
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

        /// <summary>
        /// Get the current state of caffeine for display
        /// </summary>
        /// <returns>String indicating next action (On/Off)</returns>
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
        
        /// <summary>
        /// Initialize the plugin
        /// </summary>
        /// <param name="context">Plugin initialization context</param>
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
                if (TrayIconManager.IsInitialized)
                {
                    TrayIconManager.ShowTray();
                }
                _enabled = true;
            }
        }

        private void StopCaffeine()
        {
            if (_enabled)
            {
                PowerUtilities.Shutdown();
                if (TrayIconManager.IsInitialized)
                {
                    TrayIconManager.HideTray();
                }
                _enabled = false;
            }
        }

        /// <summary>
        /// Create the settings panel for the plugin
        /// </summary>
        /// <returns>The settings user control</returns>
        public System.Windows.Controls.Control CreateSettingPanel()
        {
            return new PluginSettings(Context, _settings);
        }
    }
}