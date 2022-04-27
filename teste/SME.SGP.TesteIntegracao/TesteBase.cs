using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace SME.SGP.TesteIntegracao
{
    [Collection("TesteIntegradoSGP")]
    public class TesteBase : IClassFixture<TestFixture>
    {
        protected ServiceProvider ServiceProvider => _collectionFixture.ServiceProvider;
        private readonly CollectionFixture _collectionFixture;
        public TesteBase(CollectionFixture collectionFixture)
        {
            _collectionFixture = collectionFixture;
            _collectionFixture.Database.LimparBase();
        }

        public Task InserirNaBase<T>(IEnumerable<T> objetos) where T : class, new()
        {
            _collectionFixture.Database.Inserir(objetos);
            return Task.CompletedTask;
        }

        public Task InserirNaBase<T>(T objeto) where T : class, new()
        {
            _collectionFixture.Database.Inserir(objeto);
            return Task.CompletedTask;
        }

        public List<T> ObterTodos<T>() where T : class, new()
        {
            return _collectionFixture.Database.ObterTodos<T>();
        }

        public T ObterPorId<T, K>(K id)
            where T : class, new()
            where K : struct
        {
            return _collectionFixture.Database.ObterPorId<T, K>(id);
        }

    }
}
