using Newtonsoft.Json;
using Xunit.Abstractions;

namespace LsIntegrationTests;

public static class ExtensionsForTesting
{
    public static void Dump(this object obj)
    {
        Console.WriteLine(obj.DumpAsJson());
    }

    public static string DumpAsJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static void Dump(this ITestOutputHelper console, object obj)
    {
        console.WriteLine(obj.DumpAsJson());
    }
}