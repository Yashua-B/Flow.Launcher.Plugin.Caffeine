using System.Windows.Controls;
using Flow.Launcher.Plugin.Caffeine.Tray;

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
        ShowTrayIconCheckBox.IsChecked = _settings.ShowTrayIcon;
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

    private void ShowTrayIconCheckBox_Changed(object sender, System.Windows.RoutedEventArgs e)
    {
        if (!_isLoading)
            SaveSettings();
    }

    private void SaveSettings()
    {
        _settings.StartWithFlowLauncher = StartWithFlowLauncherCheckBox.IsChecked ?? false;
        _settings.SendNotifications = SendNotificationsCheckBox.IsChecked ?? true;
        _settings.ShowTrayIcon = ShowTrayIconCheckBox.IsChecked ?? true;
        
        _context.API.SaveSettingJsonStorage<Settings>();

        if (_settings.ShowTrayIcon && Caffeine.IsActive) TrayIconManager.ShowTray(_context);
        if (!_settings.ShowTrayIcon) TrayIconManager.HideTray();
    }
}
