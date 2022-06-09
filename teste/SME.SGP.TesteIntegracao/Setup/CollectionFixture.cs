using System;
using System.Data;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class CollectionFixture : IDisposable
    {
        private readonly IServiceCollection _services;
        public readonly InMemoryDatabase Database;
        public readonly ServiceProvider ServiceProvider;
        
        public CollectionFixture()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _services = new ServiceCollection();

            Database = new InMemoryDatabase();
            _services.AddScoped<IDbConnection>(x=> Database.Conexao);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
            _services.AddSingleton<IConfiguration>(config);
            _services.AddMemoryCache();
            new RegistradorDependencias().Registrar(_services, null);
            _services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>),
                typeof(PublicarFilaSgpCommandHandlerFake), ServiceLifetime.Scoped));
            RegistraMock();
            ServiceProvider = _services.BuildServiceProvider();
            DapperExtensionMethods.Init(ServiceProvider.GetService<IServicoTelemetria>());
        }

        private void RegistraMock()
        {
            _services.AddMockPlanoAEE();
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