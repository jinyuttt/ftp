using FubarDev.FtpServer;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;
using System;
using FubarDev.FtpServer.AccountManagement;
using System.Collections.Generic;

namespace SmartFtpServer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var tool = new FtpTool();
            var cfg = tool.ReadCfg();
            var services = new ServiceCollection();
            services.Configure<DotNetFileSystemOptions>(opt => opt
               .RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFtpServer"));
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



