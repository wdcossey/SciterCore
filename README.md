![https://sciter.com/](https://sciter.com/wp-content/themes/sciter/!images/logo.png)

## About this project

SciterCore is based off the work done by [Ramon F. Mendes](https://github.com/ramon-mendes) on [SciterSharp](https://github.com/ramon-mendes/SciterSharp)

1. **Why SciterCore if SciterSharp exists?**

    SciterCore aims to bring Sciter to .Net Core developers, the end goal of this project is to build Sciter applications using .Net Core w/o the need for Xamarin/Mono.

    I have spent many hours cleaning up and tuning the project, modifying the code to make it simpler to use as well as adding support for newer versions of Sciter.

2. **Can I build .Net Core applications in the current state of the project?**

    Whilst it's possible to build .Net Core applications for Windows and MacOS, it's not recommended in its current state.
    
3. **When can I build .Net Core applications using this project?**

    I'm busy working on a custom version of [LibUI](https://github.com/andlabs/libui) to make this a reality.

## Cross-platform Sciter bindings for .NET

This library provides bindings of [Sciter](http://sciter.com/download/) C/C++ headers for C#. 

[Sciter](http://sciter.com/download/) is a multi-platform HTML engine. With this library you can create C#/.NET desktop applications using not just HTML, but all the features of Sciter: CSS3, SVG, scripting, AJAX, &lt;video&gt;

## License

[GNU General Public License v3](https://www.gnu.org/licenses/gpl-3.0.en.html)

## Available Packages

#### SciterCore.PackFolder
PackFolder MSBuild Task for embedding Sciter packed binaries into Project resources.

| Command                      |  Options                  |  Default  | Description                       |
| ---------------------------: | :------------------------ | :-------: | :-------------------------------- |
| `SciterCorePackType`         | `binary`                  | `binary`  | The PackFolder option; Only `binary` is currently supported, to disable folder packing use any other value (i.e `none`). |
| `SciterCorePackDirectory`    | absolute or relative path | `wwwroot` | Path to the folder you would like to pack.                                                                               |
| `SciterCorePackCopyToOutput` | `true` or `false`         | `false`   | Useful if you do not want to pack the files and simply want them copied to the `$(TargetDir)` during the build process.  |

Example:
```
<PropertyGroup>
  <SciterCorePackDirectory>..\common\wwwroot</SciterCorePackDirectory>
  <SciterCorePackType>binary</SciterCorePackType>
  <SciterCorePackCopyToOutput Condition=" '$(Configuration)' == 'Debug' ">true</SciterCorePackCopyToOutput>
</PropertyGroup>
```

#### SciterCore.Binaries

Package containing Sciter binaries for Windows, MacOS and Linux.

Binaries are automatically resolved for the target configuration (i.e `x86`, `x64` and `AnyCPU`)

| OS              | Architecture   | FileName              | 
| --------------- | ---------------| --------------------- | 
| Windows         | `x86`          | `sciter.dll`          | 
| Windows         | `x64`          | `sciter.dll`          | 
| MacOS           | `x64`          | `sciter-osx-64.dylib` | 
| Linux           | `x64`          | `libsciter-gtk.so`    | 

#### SciterCore.NetStd
Windows/MacOS/Linux (via .Net Standard 2.0)

#### SciterCore.WinForms
Windows, WinForms and WPF (via .Net Framework 4.6.1)

#### SciterCore.Gtk
Linux/Gtk (via MonoDevelop)

#### SciterCore.Mac
MacOS (via Xamarin.Mac)

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
