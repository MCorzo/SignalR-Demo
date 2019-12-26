using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SignalRDemoCore;

namespace SignalR_DemoCore
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://0.0.0.0:1808")
            .UseStartup<Startup>()
            .UseKestrel();
    }
}
