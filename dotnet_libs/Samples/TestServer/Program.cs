// See https://aka.ms/new-console-template for more information
using TestServer;

Console.WriteLine("Hello, World!");

var rt = new ServerRuntime();
rt.InitRuntime();
rt.Run();
rt.Stop();