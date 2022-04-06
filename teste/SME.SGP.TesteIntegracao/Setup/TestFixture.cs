using System;
using System.Data;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dados;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class TestFixture: IDisposable
    {
        private readonly IServiceCollection _services;
        public readonly InMemoryDatabase Database;

        public readonly ServiceProvider ServiceProvider;
        public TestFixture()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _services = new ServiceCollection();

            Database = new InMemoryDatabase();
            _services.TryAddScoped<IDbConnection>(x=> Database.Conexao);
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