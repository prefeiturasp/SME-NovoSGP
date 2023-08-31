using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SME.SGP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration((_, configurationBuilder) =>
                  {
                      configurationBuilder.AddEnvironmentVariables();
                      configurationBuilder.AddUserSecrets<Program>();
                  })
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}