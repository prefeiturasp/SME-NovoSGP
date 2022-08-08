using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarCachePorValorObjetoCommandHandler : IRequestHandler<SalvarCachePorValorObjetoCommand,string>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCachePorValorObjetoCommandHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> Handle(SalvarCachePorValorObjetoCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarAsync(request.NomeChave,request.Valor,request.MinutosParaExpirar,request.UtilizarGZip);
            return request.NomeChave;
        }
    }
}