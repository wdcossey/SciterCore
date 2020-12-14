![https://sciter.com/](https://github.com/wdcossey/SciterCore/raw/master/Assets/sciter_logo_grey.png)

| Package               | Version                                                                                                                 | Build Status |
| --------------------- | ------------------------------------------------------------------------------------------------------------------------|--------------|
| [SciterCore.Binaries](https://github.com/wdcossey/SciterCore.Binaries)   | [![NuGet](https://img.shields.io/nuget/v/SciterCore.Binaries)](https://www.nuget.org/packages/SciterCore.Binaries/)     | [![Build Status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore.Binaries?branchName=main)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=11&branchName=main) |
| [SciterCore.PackFolder](https://github.com/wdcossey/SciterCore.PackFolder) | [![Nuget](https://img.shields.io/nuget/v/SciterCore.PackFolder)](https://www.nuget.org/packages/SciterCore.PackFolder/) | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore.PackFolder-import)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=12&branchName=main) |
| SciterCore.NetStd     | [![Nuget](https://img.shields.io/nuget/v/SciterCore.NetStd)](https://www.nuget.org/packages/SciterCore.NetStd/)         | [![Build status](https://dev.azure.com/wdcossey/SciterCore/_apis/build/status/SciterCore)](https://dev.azure.com/wdcossey/SciterCore/_build/latest?definitionId=13&branchName=master) |
| SciterCore.WinForms   | [![Nuget](https://img.shields.io/nuget/v/SciterCore.WinForms)](https://www.nuget.org/packages/SciterCore.WinForms/)     | |
| SciterCore.Gtk        | [![Nuget](https://img.shields.io/nuget/v/SciterCore.Gtk)](https://www.nuget.org/packages/SciterCore.Gtk/)               | |
| SciterCore.Mac        | [![Nuget](https://img.shields.io/nuget/v/SciterCore.Mac)](https://www.nuget.org/packages/SciterCore.Mac/)               | |

## Status

| Operating System      | Version(s)                  | Status      | Comments |
| ----------------------|-----------------------------|-------------|-----------------------------------------------------------------------------------------|
| Windows               | Windows 10 Pro v19042.685   | Working     |                                                                                         |
| Windows               | Windows 8.x                 | Untested    |                                                                                         |
| Windows               | Windows 7                   | Untested    |                                                                                         |
| Linux                 | Ubuntu 20.04 (Hyper-V)      | Working     | Run `sudo chmod +x packfolder` in `scripts/bin.lnx` to build `SciterCore.NetStd.csproj` |
| Linux                 | Other                       | Untested    | As long as you have `libgtk-3.so.0` it's possible.                                      |
| MacOS                 | MacOS Catalina              | In-Progress |                                                                                         |
| MacOS                 | MacOS Big Sur               | Untested    |                                                                                         |

All local builds (across platforms) are done using `JetBrains Rider`.

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

#### SciterCore.PackFolder

The source can be found [here](https://github.com/wdcossey/SciterCore.PackFolder)

#### SciterCore.Binaries

The source can be found [here](https://github.com/wdcossey/SciterCore.Binaries)

#### SciterCore.NetStd
Windows/MacOS/Linux (via .Net Core 3.1)

#### SciterCore.Windows
Windows, WinForms and WPF support (via .Net Framework 4.6.1)

#### SciterCore.Gtk
Linux/Gtk support (via MonoDevelop)

#### SciterCore.Mac
MacOS support (via Xamarin.Mac)

## Available Samples

| Project                           | Description                                               | Platform(s)                 |
| --------------------------------- | :-------------------------------------------------------- | --------------------------- |
| SciterTest.NetCore                | Demo using `.Net Core 3.1`                                | `MacOS`, `Windows`          |
| SciterTest.CoreForms              | Demo using `.Net Core 3.1` and `WinForms`                 | `Windows`                   |
| SciterCore.Sample.SkiaSharp.***   | Demo using [SkiaSharp](https://github.com/mono/SkiaSharp) | `MacOS`, `Windows`          |
| SciterCore.Sample.Gtk.***         | Demo using `GTK`, migrated from `SciterSharp`             | `MacOS`, `Windows`, `Linux` |
| SciterTest.WinForms               | Demo using `.Net Framework 4.6.1` and `WinForms`          | `Windows`                   |
| SciterTest.Wpf                    | Demo using `.Net Framework 4.6.1` and `WPF`               | `Windows`                   |
| SciterTest.Core                   | Sample migrated from `SciterSharp`                        | `Windows`                   |
| SciterTest.Mac                    | Demo using `Xamarin.Mac`, migrated from `SciterSharp`     | `MacOS`                     |
| SciterTest.Idioms                 | Sample migrated from `SciterSharp`                        | `Windows`                   |

## Screenshots

Windows 10 (.Net Core 3.1.x)

![https://github.com/wdcossey/SciterCore/](https://github.com/wdcossey/SciterCore/raw/master/Assets/preview/scitercore_preview_001.png)

Ubuntu 20.04 (.Net Core 3.1.x)

![https://github.com/wdcossey/SciterCore/](https://github.com/wdcossey/SciterCore/raw/master/Assets/preview/scitercore_preview_ubuntu_001.png)
