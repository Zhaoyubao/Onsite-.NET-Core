using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Notes
{
    class Program
    {
        static void Main()
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
