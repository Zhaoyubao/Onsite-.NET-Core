using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using MySQL.Data.EntityFrameworkCore.Extensions;
using BeltExam.Models;

namespace BeltExam
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string Connection = Configuration["DBInfo:MyConnection"];
            // string Connection = Configuration.GetSection("DBInfo")["MyConnection"];
            // string Connection = Configuration.GetSection("DBInfo:MyConnection").Value;
            services.AddDbContext<MysqlContext>( options => options.UseMySQL(Connection) );
            services.AddMvc();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}
