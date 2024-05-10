using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra;

namespace SME.SGP.IoC
{
    internal static class RegistrarGoogleClassroomSync
    {
        internal static void ConfigurarGoogleClassroomSync(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<GoogleClassroomSyncOptions>()
                .Bind(configuration.GetSection(nameof(GoogleClassroomSyncOptions)), c => c.BindNonPublicProperties = true);

            services.AddSingleton<GoogleClassroomSyncOptions>();
        }
    }
}
