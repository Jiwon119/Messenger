// See https://aka.ms/new-console-template for more information
using TestClient;

Console.WriteLine("Hello, World!");

ClientRuntime rt = new ClientRuntime();
rt.InitRuntime();
rt.Run();
rt.Stop();