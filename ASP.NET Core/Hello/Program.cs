using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Demo
{
    public class Program
    {
        public static void Main()
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000", "http://localhost:8000")
                .Build();
                
            host.Run();
        }
    }
}