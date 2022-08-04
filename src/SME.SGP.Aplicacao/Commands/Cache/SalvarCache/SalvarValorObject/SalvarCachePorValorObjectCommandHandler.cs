using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarCachePorValorObjectCommandHandler : IRequestHandler<SalvarCachePorValorObjectCommand,string>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCachePorValorObjectCommandHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> Handle(SalvarCachePorValorObjectCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarAsync(request.NomeChave,request.Valor,request.MinutosParaExpirar,request.UtilizarGZip);
            return request.NomeChave;
        }
    }
}