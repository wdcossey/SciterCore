﻿using System.Reflection;
using System.Runtime.InteropServices;

#if WINDOWS
[assembly: AssemblyTitle("SciterCore")]
[assembly: AssemblyProduct("SciterCore.Windows")]
#elif GTKMONO
[assembly: AssemblyTitle("SciterCore")]
[assembly: AssemblyProduct("SciterCore.Gtk")]
#elif OSX
[assembly: AssemblyTitle("SciterCore")]
[assembly: AssemblyProduct("SciterCore.Xamarin.Mac")]
#endif

[assembly: AssemblyDescription("C# bindings for the Sciter engine")]
[assembly: AssemblyConfiguration("Retail RELEASE build")]
[assembly: AssemblyCompany("MI Software")]
[assembly: AssemblyCopyright("Copyright © MI Software 2016 - GPLv3")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components. If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("18b2e4e7-cb77-44c3-865b-be05620aba4d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.*")]// use three version numbers only to match Nuget system
[assembly: AssemblyFileVersion("1.0.*")]
