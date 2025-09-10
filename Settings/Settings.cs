using System;

namespace Flow.Launcher.Plugin.Caffeine.Settings;

/// <summary>
/// Plugin settings configuration
/// </summary>
public class Settings
{
    /// <summary>
    /// Whether to start caffeine service automatically when Flow Launcher starts
    /// </summary>
    public bool StartWithFlowLauncher { get; set; } = false;

    /// <summary>
    /// Whether to send notifications when caffeine starts/stops
    /// </summary>
    public bool SendNotifications { get; set; } = true;
}
