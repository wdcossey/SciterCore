![https://sciter.com/](https://sciter.com/wp-content/themes/sciter/!images/logo.png)

SciterCore.PackFolder [![NuGet](https://img.shields.io/nuget/v/.svg?style=flat)]() ![Build status](https://dev.azure.com/wdcossey/GitHub/_apis/build/status/SciterCore.PackFolder)

SciterCore.Binaries.Windows [![NuGet](https://img.shields.io/nuget/v/.svg?style=flat)]() ![Build status](https://dev.azure.com/wdcossey/GitHub/_apis/build/status/SciterCore.Binaries.Windows)

SciterCore.WinForms [![NuGet](https://img.shields.io/nuget/v/.svg?style=flat)]() ![Build status](https://dev.azure.com/wdcossey/GitHub/_apis/build/status/SciterCore.WinForms)

SciterCore.Gtk [![NuGet](https://img.shields.io/nuget/v/.svg?style=flat)]() ![Build status](https://dev.azure.com/wdcossey/GitHub/_apis/build/status/SciterCore.Gtk)

SciterCore.Mac [![NuGet](https://img.shields.io/nuget/v/.svg?style=flat)]() ![Build status](https://dev.azure.com/wdcossey/GitHub/_apis/build/status/SciterCore.Mac)

## About this project

SciterCore is based off the work done by [Ramon F. Mendes](https://github.com/ramon-mendes) on [SciterSharp](https://github.com/ramon-mendes/SciterSharp)

1. **Why SciterCore if SciterSharp exists?**

    SciterCore aims to bring Sciter to .Net Core developers, the end goal of this project is to build Sciter applications using .Net Core w/o the need for Xamarin/Mono.

    I have spent many hours cleaning up and tuning the project, modifying the code to make it simpler to use as well as adding support for newer versions of Sciter.

2. **Can I build .Net Core applications in the current state of the project?**

    Whilst it's possible to build .Net Core applications for Windows using some PInvoke, it's not recommended in its current state.
    
3. **When can I build .Net Core applications using this project?**

    I'm busy working on a custom version of [LibUI](https://github.com/andlabs/libui) to make this a reality.

## Cross-platform Sciter bindings for .NET

This library provides bindings of [Sciter](http://sciter.com/download/) C/C++ headers for C#. 

[Sciter](http://sciter.com/download/) is a multi-platform HTML engine. With this library you can create C#/.NET desktop applications using not just HTML, but all the features of Sciter: CSS3, SVG, scripting, AJAX, &lt;video&gt;

## License

[GNU General Public License v3](https://www.gnu.org/licenses/gpl-3.0.en.html)

## Available Packages

##### SciterCore.PackFolder
PackFolder build Tasks for embedding Sciter packed binaries into Project resources.

##### SciterCore.Binaries.Windows
Package for x86 and x64 Windows binaries

##### SciterCore.WinForms
Windows/WinForms

##### SciterCore.Gtk
Linux/Gtk (via MonoDevelop)

##### SciterCore.Mac
MacOS (via Xamarin.Mac)

## Available Samples

_SciterTest.Graphics_ -- Windows sample using [SkiaSharp](https://github.com/mono/SkiaSharp)

_SciterTest.Graphics.Mac_ -- MacOS sample using [SkiaSharp](https://github.com/mono/SkiaSharp)

_SciterTest.Gtk_ -- Linux sample using GTK

_SciterTest.Gtk.Mac_ -- MacOS sample using GTK

_SciterTest.Gtk.Windows_ -- Windows sample using GTK

_SciterTest.CoreForms_ -- .Net Core 3.1 Windows sample using a custom WinForms component

_SciterTest.Core_ -- Windows core sample

_SciterTest.Mac_ -- MacOS core sample

_SciterTest.NetCore_ -- .Net Core 3.1 Windows sample

_SciterTest.WinForms_ -- .Net Framework (4.6.x) Windows sample using a custom WinForms component.

_SciterTest.Wpf_ -- Windows core sample