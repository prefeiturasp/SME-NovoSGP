using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dados;
using SME.SGP.Infra;
using System;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dados.Contexto;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class TestFixture : IDisposable
    {
        private readonly IServiceCollection _services;
        public readonly InMemoryDatabase Database;
        public readonly ServiceProvider ServiceProvider;
        public TestFixture()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _services = new ServiceCollection();

            Database = new InMemoryDatabase();
            _services.AddScoped<IDbConnection>(x=> Database.Conexao);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
            _services.AddSingleton<IConfiguration>(config);
            _services.AddMemoryCache();
            new RegistradorDependencias().Registrar(_services, null);

            ServiceProvider = _services.BuildServiceProvider();
            DapperExtensionMethods.Init(ServiceProvider.GetService<IServicoTelemetria>());
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}