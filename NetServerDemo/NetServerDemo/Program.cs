﻿// See https://aka.ms/new-console-template for more information
using NetServerDemo;

Console.WriteLine("Hello, World!");

//var rt = new ServerRuntime();
//rt.InitRuntime();
//rt.Run();
//rt.Stop();

ServerRuntime.Instance.InitRuntime();
ServerRuntime.Instance.Run();
ServerRuntime.Instance.Stop();