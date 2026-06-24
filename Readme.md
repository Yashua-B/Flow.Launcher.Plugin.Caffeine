Caffeine for Flow Launcher
==================
A plugin for [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) that prevents your pc from sleeping or turning off the monitor.  
This is a replacement for [Caffeine](https://www.zhornsoftware.co.uk/caffeine/) that simply keeps your pc awake and screen on when active.  

[![Download](https://img.shields.io/badge/⬇%20Download%20latest-Caffeine-2ea44f?style=for-the-badge&logo=github)](https://github.com/Yashua-B/Flow.Launcher.Plugin.Caffeine/releases/latest)

> Click the button above to grab the latest ready-to-use `.zip` — no compiling needed. See [Installation](#installation) below.

Icons from [icons8](https://icons8.com/).  
Thanks for that one guy on stackexchange that had a nice clean example of how power management works.

### Usage

Type `caf` into [Flow Launcher](https://github.com/Flow-Launcher/Flow.Launcher) to choose how long to keep your PC awake. Press enter to activate — the first option (indefinitely) works just like the original toggle.

![caffeine presets](Images/readme/caff-presets.png)

You can also type a custom duration directly:

| Command | Effect |
|---------|--------|
| `caf` | Show duration presets (indefinite, 30m, 1h, 2h, 8h) |
| `caf 3` | Keep awake for 3 hours |
| `caf 45m` | Keep awake for 45 minutes |
| `caf 1.5` | Keep awake for 1.5 hours (90 minutes) |
| `caf off` | Turn off caffeine |

When caffeine is already active, typing `caf` shows a "Turn off" option at the top along with options to switch to a different duration. The current mode is hidden from the list since you're already using it.

### Tray Icon

Right-click the tray icon for quick access to duration presets and turn off. The remaining time is shown at the top of the menu and in the tooltip on hover.

![caffeine tray](Images/readme/caff-tray.png)

### Settings

![caffeine settings](Images/readme/caff-settings.png)

> **Note:**  
> The image above show the default settings for the plugin.

## Installation

Make sure [Flow Launcher](https://github.com/Flow-Launcher/Flow.Launcher) is installed first.

### Option 1 — Download (recommended)

1. Download the latest [`Caffeine-x.x.x.zip`](https://github.com/Yashua-B/Flow.Launcher.Plugin.Caffeine/releases/latest).
2. Unzip it into your Flow Launcher plugins folder: `%APPDATA%\FlowLauncher\Plugins`
3. Restart Flow Launcher.

### Option 2 — Plugin store

Execute the following command in the [Flow Launcher](https://github.com/Flow-Launcher/Flow.Launcher) query:

```cmd
pm install Caffeine by o850cHQk
```

or

Search for `Caffeine` within [flow launchers](https://github.com/Flow-Launcher/Flow.Launcher) plugin store
