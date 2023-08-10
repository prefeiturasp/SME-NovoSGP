using Microsoft.Extensions.Caching.Memory;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheMemoria : RepositorioCache
    {
        private readonly IMemoryCache memoryCache;

        public RepositorioCacheMemoria(IMemoryCache memoryCache, IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs) 
            : base(servicoTelemetria, servicoMensageriaLogs)
        {
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

            NomeServicoCache = "Cache Memória";
        }

        protected override string ObterValor(string nomeChave)
            => memoryCache.Get<string>(nomeChave);

        protected override Task RemoverValor(string nomeChave)
            => Task.Run(() => memoryCache.Remove(nomeChave));

        protected override Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
            => Task.Run(() => memoryCache.Set(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar)));
    }
}
