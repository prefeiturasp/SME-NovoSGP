using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper.FluentMap;
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
            _services.AddScoped<IDbConnection>(x => Database.Conexao);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
            _services.AddSingleton<IConfiguration>(config);
            _services.AddMemoryCache();
            FluentMapper.EntityMaps.Clear();
            new RegistradorDependencias().Registrar(_services, null);
            RegistraMock();
            ServiceProvider = _services.BuildServiceProvider();
            DapperExtensionMethods.Init(ServiceProvider.GetService<IServicoTelemetria>());
        }

        private void RegistraMock()
        {
            _services.AddMockPlanoAEE();
            _services.AddMockFila();
            _services.AddMockAluno();
            CarregarMockAtribuicaoReponsaveis();
        }

        public void CarregarMockAtribuicaoReponsaveis()
        {
            _services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSupervisorPorCodigoQuery, IEnumerable<SupervisoresRetornoDto>>), typeof(ObterSupervisorPorCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            _services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            _services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));

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