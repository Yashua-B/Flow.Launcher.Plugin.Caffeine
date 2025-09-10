using System.Windows.Controls;

namespace Flow.Launcher.Plugin.Caffeine.Settings;

public partial class PluginSettings : UserControl
{
    private readonly PluginInitContext _context;
    private readonly Settings _settings;
    private bool _isLoading;

    /// <summary>
    /// Initialize the plugin settings UI
    /// </summary>
    /// <param name="context">Plugin context</param>
    /// <param name="settings">Plugin settings</param>
    public PluginSettings(PluginInitContext context, Settings settings)
    {
        InitializeComponent();
        _context = context;
        _settings = settings;
        LoadSettings();
    }

    private void LoadSettings()
    {
        _isLoading = true;
        StartWithFlowLauncherCheckBox.IsChecked = _settings.StartWithFlowLauncher;
        SendNotificationsCheckBox.IsChecked = _settings.SendNotifications;
        _isLoading = false;
    }
    
    private void StartWithFlowLauncherCheckBox_Changed(object sender, System.Windows.RoutedEventArgs e)
    {
        if (!_isLoading)
            SaveSettings();
    }

    private void SendNotificationsCheckBox_Changed(object sender, System.Windows.RoutedEventArgs e)
    {
        if (!_isLoading)
            SaveSettings();
    }

    private void SaveSettings()
    {
        _settings.StartWithFlowLauncher = StartWithFlowLauncherCheckBox.IsChecked ?? false;
        _settings.SendNotifications = SendNotificationsCheckBox.IsChecked ?? false;
        _context.API.SaveSettingJsonStorage<Settings>();
    }
}
