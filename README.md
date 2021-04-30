![https://sciter.com/](https://github.com/wdcossey/SciterCore/raw/master/Assets/sciter_logo_grey.png)

| Package               | Version                                                                                                                 | Build Status |
| --------------------- | ------------------------------------------------------------------------------------------------------------------------|--------------|
| SciterCore            | [![Nuget](https://img.shields.io/nuget/v/SciterCore)](https://www.nuget.org/packages/SciterCore/)                       | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=13&branchName=master) |
| SciterCore.Microsoft.DependencyInjection | [![Nuget](https://img.shields.io/nuget/v/SciterCore.Microsoft.DependencyInjection)](https://www.nuget.org/packages/SciterCore.Microsoft.DependencyInjection/)                 | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=13&branchName=master) |
| SciterCore.JS         | [![Nuget](https://img.shields.io/nuget/v/SciterCore.JS)](https://www.nuget.org/packages/SciterCore.JS/)                 | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=13&branchName=master) |
| SciterCore.JS.Microsoft.DependencyInjection | [![Nuget](https://img.shields.io/nuget/v/SciterCore.JS.Microsoft.DependencyInjection)](https://www.nuget.org/packages/SciterCore.JS.Microsoft.DependencyInjection/)                 | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=13&branchName=master) |
| SciterCore.SciterSharp.Utilities | [![Nuget](https://img.shields.io/nuget/v/SciterCore.SciterSharp.Utilities)](https://www.nuget.org/packages/SciterCore.SciterSharp.Utilities/)                 | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=13&branchName=master) |
| SciterCore.Windows    | [![Nuget](https://img.shields.io/nuget/v/SciterCore.WinForms)](https://www.nuget.org/packages/SciterCore.WinForms/)     | |
| SciterCore.Gtk        | [![Nuget](https://img.shields.io/nuget/v/SciterCore.Gtk)](https://www.nuget.org/packages/SciterCore.Gtk/)               | |
| SciterCore.Xamarin.Mac| [![Nuget](https://img.shields.io/nuget/v/SciterCore.Mac)](https://www.nuget.org/packages/SciterCore.Mac/)               | |
| [SciterCore.Binaries](https://github.com/wdcossey/SciterCore.Binaries)   | [![NuGet](https://img.shields.io/nuget/v/SciterCore.Binaries)](https://www.nuget.org/packages/SciterCore.Binaries/)     | |
| [SciterCore.JS.Binaries](https://github.com/wdcossey/SciterCore.JS.Binaries)   | [![NuGet](https://img.shields.io/nuget/v/SciterCore.JS.Binaries)](https://www.nuget.org/packages/SciterCore.JS.Binaries/)     | |
| [SciterCore.PackFolder](https://github.com/wdcossey/SciterCore.PackFolder) | [![Nuget](https://img.shields.io/nuget/v/SciterCore.PackFolder)](https://www.nuget.org/packages/SciterCore.PackFolder/) | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore.PackFolder)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=12) |

## Status

| Operating System      | Version(s)                        | .Net                           | Status      | Comments |
| ----------------------|-----------------------------------|--------------------------------|-------------|-----------------------------------------------------------------------------------------|
| Windows               | Windows 10 Pro `v19042.685`       | .Net Core `3.1.10` & `5.0.102` | Working     |                                                                                         |
| Windows               | Windows 8.x                       | N/A                            | Untested    |                                                                                         |
| Windows               | Windows 7 Pro SP1 `v7601`         | .Net Core `3.1.404`            | Working     | Ensure you have updated your OS!                                                        |
| Linux                 | Ubuntu `20.04` <sup>proxmox</sup> | .Net Core `3.1.10`             | Working     | Run `sudo chmod +x packfolder` in `scripts/bin.lnx` to build `SciterCore.csproj`        |
| Linux                 | Fedora `33` <sup>proxmox</sup>    | .Net Core `3.1.10`             | Working     | Run `sudo chmod +x packfolder` in `scripts/bin.lnx` to build `SciterCore.csproj`        |
| Linux                 | Manjaro `XFCE` <sup>proxmox</sup> | .Net Core `3.1.10`             | Working     | Run `sudo chmod +x packfolder` in `scripts/bin.lnx` to build `SciterCore.csproj`        |
| Linux                 | Other                             | N/A                            | Untested    | As long as you have `libgtk-3.so.0`.                                                    |
| MacOS                 | MacOS Catalina                    | N/A                            | In-Progress |                                                                                         |
| MacOS                 | MacOS Big Sur                     | N/A                            | Untested    |                                                                                         |

All builds are done using `JetBrains Rider` or _vanilla_ `dotnet` CLI.

The changes are going to be frequent while I add stop-gaps between adding/enhancing functionality, expect things to break!!!

Join in, start building, testing, help the project find and fix bugs, add/suggest new functionality.

## About this project

SciterCore is based off the work done by [Ramon F. Mendes](https://github.com/ramon-mendes) on [SciterSharp](https://github.com/ramon-mendes/SciterSharp)

** `SciterCore` is `NOT` backwards compatible with `SciterSharp`, you can't simply change the packages and hope things will work, changes will need to be made.

1. **Why SciterCore if SciterSharp exists?**

    SciterCore aims to bring Sciter to .Net Core developers, the end goal of this project is to build Sciter applications using .Net Core w/o the need for Xamarin/Mono.
  
    While I will do my best to add support for WinForms, WPF, Xamarin, etc. these are not high priority, the goal is .Net Core (and .Net 5+).

    I have spent many hours cleaning up and tuning the project, modifying the code to make it simpler to use as well as adding support for newer versions of Sciter.
    
2. **Can I build production .Net Core applications in the current state of the project?**

    Whilst it's possible to build .Net Core applications for Windows, Linux and MacOS, it's not recommended in its current state.
    
3. **When can I build .Net Core applications using this project?**
    
    I'm busy working on a custom version of [LibUI](https://github.com/andlabs/libui) to make this a reality.

## Cross-platform Sciter bindings for .NET

This library provides bindings of [Sciter](http://sciter.com/download/) C/C++ headers for C#. 

[Sciter](http://sciter.com/download/) is a multi-platform HTML engine. With this library you can create C#/.NET desktop applications using not just HTML, but all the features of Sciter: CSS3, SVG, scripting, AJAX, &lt;video&gt;

## License

[GNU General Public License v3](https://www.gnu.org/licenses/gpl-3.0.en.html)

## Available Packages

### SciterCore (previously `SciterCore.NetStd`)
Windows/MacOS/Linux via `.Net Core 3.1` & `.Net 5`.

### SciterCore.JS
Windows/MacOS/Linux via `.Net Core 3.1` & `.Net 5`, using Sciter.JS (JavaScript).

### SciterCore.Windows  (previously `SciterCore.WinForms`)
Windows, WinForms and WPF support via `.Net Framework 4.6.1`

### SciterCore.Gtk
Linux/Gtk support via `MonoDevelop`

### SciterCore.Xamarin.Mac (previously `SciterCore.Mac`)
MacOS support via `Xamarin.Mac`

### SciterCore.PackFolder

The source can be found [here](https://github.com/wdcossey/SciterCore.PackFolder)

### SciterCore.Binaries

The source can be found [here](https://github.com/wdcossey/SciterCore.Binaries)

### SciterCore.JS.Binaries

The source can be found [here](https://github.com/wdcossey/SciterCore.JS.Binaries)

## Available Samples

| Project                           | Description                                               | Platform(s)                 |
| --------------------------------- | :-------------------------------------------------------- | --------------------------- |
| SciterCore.JS.HelloSciter         | `SciterCore.JS` demo using `.Net 5.0.x`                   | `MacOS`, `Windows`, `Linux` |
| SciterCore.HelloSciter            | `SciterCore` demo using `.Net Core 3.1.x`                 | `MacOS`, `Windows`, `Linux` |
| SciterCore.Behaviors              | `SciterCore` Behaviors Demo using `.Net Core 3.1.x`       | `MacOS`, `Windows`, `Linux` |
| SciterCore.Playground             | `SciterCore` playground project using `.Net Core 3.1.x`   | `MacOS`, `Windows`, `Linux` |
| SciterCore.CoreForms              | `SciterCore` Demo using `.Net Core 3.1.x` w/ `WinForms`   | `Windows`                   |
| SciterCore.Sample.SkiaSharp.***   | Demo using [SkiaSharp](https://github.com/mono/SkiaSharp) | `MacOS`, `Windows`          |
| SciterCore.Sample.Gtk.***         | Demo using `GTK`, migrated from `SciterSharp`             | `MacOS`, `Windows`, `Linux` |
| SciterCore.Windows.WinForms       | Demo using `.Net Framework 4.6.x` w/ `WinForms`           | `Windows`                   |
| SciterCore.Windows.Wpf            | Demo using `.Net Framework 4.6.x` w/ `WPF`                | `Windows`                   |
| SciterCore.Windows.Core           | Migrated from `SciterSharp`, using `.Net Framework 4.6.x` | `Windows`                   |
| SciterCore.Xamarin.Mac.HelloSciter| Migrated from `SciterSharp`, using `Xamarin.Mac`          | `MacOS`                     |

## Screenshots

Windows 10 (.Net Core 3.1.x)

![https://github.com/wdcossey/SciterCore/](https://github.com/wdcossey/SciterCore/raw/master/Assets/preview/scitercore_preview_001.png)

![https://github.com/wdcossey/SciterCore/](https://github.com/wdcossey/SciterCore/raw/master/Assets/preview/clock_behaviors.gif)

Ubuntu 20.04 (.Net Core 3.1.x)

![https://github.com/wdcossey/SciterCore/](https://github.com/wdcossey/SciterCore/raw/master/Assets/preview/scitercore_preview_ubuntu_001.png)
