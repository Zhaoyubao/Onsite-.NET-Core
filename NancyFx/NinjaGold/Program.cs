﻿using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NinjaGold
{
    public class Program
    {
        public static void Main()
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
