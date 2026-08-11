using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class CollectionFixture : IAsyncLifetime, IDisposable
    {
        public IServiceCollection Services { get; set; }
        public InMemoryDatabase Database { get; private set; }
        public ServiceProvider ServiceProvider { get; set; }

        public CollectionFixture()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public async Task InitializeAsync()
        {
            Database = new InMemoryDatabase();
            await Database.InitializeAsync();
            
            IniciarServicos();
            BuildServiceProvider();
        }

        public async Task DisposeAsync()
        {
            if (Database != null)
                await Database.DisposeAsync();
        }

        public void ExecutarScripts(List<ScriptCarga> scriptsCarga)
        {
            Database.ExecutarScripts(scriptsCarga);
        }

        public void IniciarServicos()
        {
            Services = new ServiceCollection();

            Services.AddScoped<IDbConnection>(_ => Database.Conexao);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
            Services.AddSingleton<IConfiguration>(config);

            Services.AddMemoryCache();
            FluentMapper.EntityMaps.Clear();

            var culture = CultureInfo.CreateSpecificCulture("pt-BR");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var registraDependencias = new RegistradorDependenciasTeste();
            registraDependencias.Registrar(Services, config);
            registraDependencias.RegistrarGoogleClassroomSync(Services, config);
            registraDependencias.RegistrarHttpClients(Services, config);
            registraDependencias.RegistrarPolicies(Services);
        }

        public void BuildServiceProvider()
        {
            ServiceProvider = Services.BuildServiceProvider();
            DapperExtensionMethods.Init(ServiceProvider.GetService<IServicoTelemetria>());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    [CollectionDefinition("TesteIntegradoSGP")]
    public class CollectionDoTeste : ICollectionFixture<CollectionFixture>
    {
    }
}