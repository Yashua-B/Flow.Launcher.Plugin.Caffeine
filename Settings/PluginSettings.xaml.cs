using System.Windows.Controls;

namespace Flow.Launcher.Plugin.Caffeine.Settings;

public partial class PluginSettings : UserControl
{
    private readonly PluginInitContext _context;
    private readonly Settings _settings;

    public PluginSettings(PluginInitContext context, Settings settings)
    {
        InitializeComponent();
        _context = context;
        _settings = settings;
        LoadSettings();
    }

    private void LoadSettings()
    {
        StartWithFlowLauncherCheckBox.IsChecked = _settings.StartWithFlowLauncher;
    }
    
    private void StartWithFlowLauncherCheckBox_Changed(object sender, System.Windows.RoutedEventArgs e)
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        _settings.StartWithFlowLauncher = StartWithFlowLauncherCheckBox.IsChecked ?? false;
        _context.API.SaveSettingJsonStorage<Settings>();
    }
}
