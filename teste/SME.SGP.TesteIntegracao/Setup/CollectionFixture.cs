﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dados;
using SME.SGP.Infra;
using System;
using System.Data;
using System.Text;
using Dapper.FluentMap;
using Xunit;

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

        public void IniciarServicos()
        {
            Services = new ServiceCollection();

            Services.AddScoped<IDbConnection>(x => Database.Conexao);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();

            Services.AddSingleton<IConfiguration>(config);
            Services.AddMemoryCache();
            
            FluentMapper.EntityMaps.Clear();
            
            new RegistradorDependencias().Registrar(Services, null);

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
