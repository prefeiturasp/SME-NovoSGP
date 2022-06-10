using System;
using Microsoft.Extensions.DependencyInjection;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class WorkerServiceScopeFactoryFake : IServiceScopeFactory
    {
        private readonly ServiceProvider _provider;

        public WorkerServiceScopeFactoryFake(ServiceProvider provider)
        {
            _provider = provider;
        }

        public IServiceScope CreateScope()
        {
            return new WorkerServiceScopeFake(_provider);
        }
    }
    
    public class WorkerServiceScopeFake : IServiceScope
    {
        private readonly ServiceProvider _provider;

        public WorkerServiceScopeFake(ServiceProvider provider)
        {
            _provider = provider;
        }

        public void Dispose()
        {
        }

        public IServiceProvider ServiceProvider => _provider;
    }
}