using Newtonsoft.Json;
using SME.SGP.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.IntegracaoAPI.Config
{
    [CollectionDefinition(nameof(TesteIntegracaoApiFixtureCollection))]
    public class TesteIntegracaoApiFixtureCollection : ICollectionFixture<TesteIntegracaoFixture<Startup>>
    {
    }
    public class TesteIntegracaoFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly SgpApiAppFactory<TStartup> Factory;
        public HttpClient Client;
        public TesteIntegracaoFixture()
        {
            var clientOptions = new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions()
            {
                HandleCookies = false,
                BaseAddress = new Uri("http://localhost/api/v1/"),
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new SgpApiAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }
        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
