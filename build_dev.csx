using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Web;

bool ExecuteAndCheckResult(string command, string args)
{
	var process = Process.Start(new ProcessStartInfo(command, args));
	process.WaitForExit();
	return process.ExitCode == 0;
}

Console.WriteLine("ingen Easy BuildTool v3");

Console.WriteLine("Checking BuildTarget...");

if (!Directory.Exists(@"src\KyoshinEewViewer"))
{
	Console.WriteLine("Project folder not found.");
	Environment.Exit(1);
	return;
}

if (Directory.Exists(@"src\KyoshinEewViewer\bin\Debug\netcoreapp3.1\win10-x64\publish"))
{
	Console.WriteLine("Cleaning...");
	foreach (var f in Directory.GetFiles(@"src\KyoshinEewViewer\bin\Debug\netcoreapp3.1\win10-x64\publish"))
		File.Delete(f);
	Directory.Delete(@"src\KyoshinEewViewer\bin\Debug\netcoreapp3.1\win10-x64\publish");
}

Console.WriteLine("Building...");
if (!ExecuteAndCheckResult("dotnet", $"publish src/KyoshinEewViewer/KyoshinEewViewer.csproj -r win10-x64 -c Debug"))
{
	Console.WriteLine("BuildFailed");
	Environment.Exit(1);
	return;
}
if (!Directory.Exists("out"))
	Directory.CreateDirectory("out");
File.Copy(@"src\KyoshinEewViewer\bin\Debug\netcoreapp3.1\win10-x64\publish\KyoshinEewViewer.exe", @"out\KyoshinEewViewer.exe", true);

Console.WriteLine("Build completed!");