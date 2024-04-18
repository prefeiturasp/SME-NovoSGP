using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dados;
using SME.SGP.Infra;
using System;
using System.Data;
using System.Text;
using Dapper.FluentMap;
using Xunit;
using System.Globalization;
using Elastic.Apm.Api;
using MediatR;
using System.Reflection;
using SME.SGP.TesteIntegracao.Commands;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class CollectionFixture : IDisposable
    {
        public IServiceCollection Services { get; set; }
        public InMemoryDatabase Database { get; }
        public ServiceProvider ServiceProvider { get; set; }

        public CollectionFixture()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Database = new InMemoryDatabase();

            IniciarServicos();
        }

        public void ExecutarScripts()
        {
            Database.ExecutarScripts();
        }

        public void IniciarServicos()
        {
            Services = new ServiceCollection();

            Services.AddScoped<IDbConnection>(x => Database.Conexao);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();

            Services.AddSingleton<IConfiguration>(config);
            Services.AddMemoryCache();
            
            FluentMapper.EntityMaps.Clear();

            var culture = CultureInfo.CreateSpecificCulture("pt-BR");

            CultureInfo.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            new RegistradorDependencias().Registrar(Services, config);
        }

        public void BuildServiceProvider()
        {
            ServiceProvider = Services.BuildServiceProvider();
            DapperExtensionMethods.Init(ServiceProvider.GetService<IServicoTelemetria>());
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }

    [CollectionDefinition("TesteIntegradoSGP")]
    public class CollectionDoTeste : ICollectionFixture<CollectionFixture>
    {
    }
}
