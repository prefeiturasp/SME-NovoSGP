using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Api;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Infra;
using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class TestFixture : IDisposable
    {
        private readonly IServiceCollection _services;
        public readonly InMemoryDatabase Database;
        public readonly ServiceProvider ServiceProvider;
        public HttpClient Client { get; set; }
        public readonly SGPFactory<Startup> Factory;
        private readonly SgpContext Context;
        public TestFixture()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _services = new ServiceCollection();

            Database = new InMemoryDatabase();
            _services.TryAddScoped<IDbConnection>(x=> Database.Conexao);
            new RegistradorDependencias().Registrar(_services, null);

            ServiceProvider = _services.BuildServiceProvider();
            DapperExtensionMethods.Init(ServiceProvider.GetService<IServicoTelemetria>());


            Factory = new SGPFactory<Startup>();
            Context = (SgpContext)Factory.Services.GetService(typeof(SgpContext));
            Client = Factory.CreateClient();

        }
        public UsuarioAutenticacaoRetornoDto Autenticar(string login,string senha)
        {
            var usuarioAutenticacao = new AutenticacaoDto
            {
                Login = login,
                Senha = senha,
            };
            var response =  Client.PostAsJsonAsync("api/v1/autenticacao", usuarioAutenticacao).Result;
            response.EnsureSuccessStatusCode();

            UsuarioAutenticacaoRetornoDto usuarioRetorno = response.Content.ReadAsJsonAsync<UsuarioAutenticacaoRetornoDto>().Result;
            return usuarioRetorno;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}