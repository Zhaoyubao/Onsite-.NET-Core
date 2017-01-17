using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace LogReg
{
    class Program
    {
        static void Main()
        {
            IWebHost host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
