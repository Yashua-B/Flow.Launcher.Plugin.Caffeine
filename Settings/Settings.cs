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
}
