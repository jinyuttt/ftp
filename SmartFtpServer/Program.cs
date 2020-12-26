using FubarDev.FtpServer;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;
using System;
using FubarDev.FtpServer.AccountManagement;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

namespace SmartFtpServer
{

    class Program
    {
       
        public static IConfiguration Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", reloadOnChange: true, optional: true)
             .AddEnvironmentVariables()
             .Build();
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .CreateLogger();
            var tool = new FtpTool();
            var cfg = tool.ReadCfg();
            var services = new ServiceCollection();
          services.AddLogging(opt => {
              opt.AddConsole()
               
               .AddConfiguration(Configuration);
               
               }
            );
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            services.Configure<DotNetFileSystemOptions>(opt => opt
               .RootPath = Path.Combine(directory.Root.Name, "FtpServer"));
            CustomMembershipProvider.Users = new List<UserInfo>()
           {
               new UserInfo(){ UserName=cfg.UserName, Password=cfg.Password}
           };

            services.AddSingleton<IMembershipProvider, CustomMembershipProvider>();
            services.AddFtpServer(builder => builder
               .UseDotNetFileSystem() 
               .EnableAnonymousAuthentication()
              
              );
          
            services.Configure<FtpConnectionOptions>(opt => { opt.InactivityTimeout = new TimeSpan(0, 0, 10); });
            services.Configure<FtpServerOptions>(opt =>
            {
                opt.ServerAddress = cfg.Address;
                opt.Port = cfg.Port;
                opt.ConnectionInactivityCheckInterval= new TimeSpan(0, 0, 10);
                opt.MaxActiveConnections = (int)(Environment.ProcessorCount * 1.5);
                
            });

            

            using (var serviceProvider = services.BuildServiceProvider(true))
            {
             
             
                var ftpServerHost = serviceProvider.GetRequiredService<IFtpServerHost>();
               
               
                await ftpServerHost.StartAsync(CancellationToken.None).ConfigureAwait(false);

                Console.WriteLine("Press ENTER/RETURN to close the test application.");
                Console.ReadLine();

              
                await ftpServerHost.StopAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}



