using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class TesteBase : IClassFixture<TestFixture>
    {
        protected ServiceProvider ServiceProvider => _testFixture.ServiceProvider;
        private readonly TestFixture _testFixture;
        public TesteBase(TestFixture testFixture)
        {
            _testFixture = testFixture;
            _testFixture.Database.LimparBase();
        }

        public Task InserirNaBase<T>(IEnumerable<T> objetos) where T : class, new()
        {
            _testFixture.Database.Inserir(objetos);
            return Task.CompletedTask;
        }

        public Task InserirNaBase<T>(T objeto) where T : class, new()
        {
            _testFixture.Database.Inserir(objeto);
            return Task.CompletedTask;
        }

        public List<T> ObterTodos<T>() where T: class, new()
        {
            return _testFixture.Database.ObterTodos<T>();
        }
        
        public T ObterPorId<T,K>(K id) 
            where T: class, new()
            where K : struct 
        {
            return _testFixture.Database.ObterPorId<T, K>(id);
        }

    }
}
