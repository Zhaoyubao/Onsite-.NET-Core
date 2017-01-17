using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DojoSurvey
{
    public class Program
    {
        public static void Main()
        {
            new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000", "http://localhost:8000")
                .Build()
                .Run();
        }
    }
}