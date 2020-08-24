using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init logs");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {              
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
        
                NLog.LogManager.Shutdown();
            }       
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          })
          .ConfigureLogging(logging =>
          {
              logging.ClearProviders();
              logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
          })
         .UseNLog();  // NLog: Setup NLog for Dependency injection
    }
}
