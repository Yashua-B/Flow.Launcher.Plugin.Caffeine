using System;
using System.Collections.Generic;
using Flow.Launcher.Plugin.Caffeine.Settings;
using Flow.Launcher.Plugin.Caffeine.Tray;

namespace Flow.Launcher.Plugin.Caffeine
{
    /// <summary>
    /// Main plugin class for the Caffeine Flow Launcher plugin
    /// </summary>
    public class Caffeine : IPlugin, ISettingProvider, IDisposable
    {
        private PluginInitContext _context;
        private Settings.Settings _settings;
        private bool _enabled = false;
        
        /// <summary>
        /// Initialize the plugin
        /// </summary>
        /// <param name="context">Plugin initialization context</param>
        public void Init(PluginInitContext context)
        {
            _context = context;
            _settings = context.API.LoadSettingJsonStorage<Settings.Settings>();

            // Start caffeine automatically if the setting is enabled
            if (_settings.StartWithFlowLauncher)
            {
                StartCaffeine();
            }
        }

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
        private string CaffeineState()
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

        /// <summary>
        /// Start the caffeine service
        /// </summary>
        private void StartCaffeine()
        {
            if (!_enabled)
            {
                PowerUtilities.PreventPowerSave();
                TrayIconManager.ShowTray(_context);
                _enabled = true;
            }
        }

        /// <summary>
        /// Stop the caffeine service
        /// </summary>
        private void StopCaffeine()
        {
            if (_enabled)
            {
                PowerUtilities.Shutdown();
                TrayIconManager.HideTray();
                _enabled = false;
            }
        }

        /// <summary>
        /// Create the settings panel for the plugin
        /// </summary>
        /// <returns>The settings user control</returns>
        public System.Windows.Controls.Control CreateSettingPanel()
        {
            return new PluginSettings(_context, _settings);
        }

        /// <summary>
        /// Dispose of resources when the plugin is unloaded
        /// </summary>
        public void Dispose()
        {
            StopCaffeine();
        }
    }
}