using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Mocks;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

//[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace SME.SGP.TesteIntegracao
{
    [Collection("TesteIntegradoSGP")]
    public class TesteBase : BaseMock, IClassFixture<TestFixture>
    {
        protected readonly CollectionFixture _collectionFixture;

        public ServiceProvider ServiceProvider => _collectionFixture.ServiceProvider;

        public TesteBase(CollectionFixture collectionFixture)
        {
            _collectionFixture = collectionFixture;
            _collectionFixture.Iniciar();
        }

        protected virtual void RegistrarFakes(IServiceCollection services)
        {
            _collectionFixture.RegistrarFakes(services);          
        }

        protected virtual void RegistrarQueryFakes(IServiceCollection services)
        {
            _collectionFixture.RegistrarQueryFakes(services);
        }

        public Task InserirNaBase<T>(IEnumerable<T> objetos) where T : class, new()
        {
            return _collectionFixture.InserirNaBase(objetos);
        }

        public Task InserirNaBase<T>(T objeto) where T : class, new()
        {
            return _collectionFixture.InserirNaBase(objeto);
        }

        public Task<long> InserirNaBaseAsync<T>(T objeto) where T : class, new()
        {
            return _collectionFixture.InserirNaBaseAsync(objeto);
        }

        public Task AtualizarNaBase<T>(T objeto) where T : class, new()
        {
            return _collectionFixture.AtualizarNaBase(objeto);
        }

        public Task InserirNaBase(string nomeTabela, params string[] campos)
        {
            return _collectionFixture.InserirNaBase(nomeTabela, campos);
        }
        
        public Task InserirNaBase(string nomeTabela, string[] campos, string[] valores)
        {
            return _collectionFixture.InserirNaBase(nomeTabela, campos, valores);
        }

        public List<T> ObterTodos<T>() where T : class, new()
        {
            return _collectionFixture.ObterTodos<T>();
        }

        public long ObterProximoId<T>() where T : EntidadeBase, new()
        {
            return ObterUltimoId<T>() + 1;
        }

        public long ObterUltimoId<T>() where T : EntidadeBase, new()
        {
            return _collectionFixture.ObterUltimoId<T>();
        }

        public T ObterPorId<T, K>(K id)
            where T : class, new()
            where K : struct
        {
            return _collectionFixture.ObterPorId<T, K>(id);
        }

        public void ExecutarScripts(List<ScriptCarga> scriptsCarga)
        {
            _collectionFixture.ExecutarScripts(scriptsCarga);
        }
    }
}
