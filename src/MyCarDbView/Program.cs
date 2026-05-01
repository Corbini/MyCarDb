
namespace MyCarDbView;

class Program
{

    static async Task Main(string[] args)
    {
#if DEBUG
        // Enable hot reload in debug mode
        System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += ctx =>
        {
            Console.WriteLine("Hot reload preparing...");
        };
#endif
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
