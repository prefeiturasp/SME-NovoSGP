using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoAvaliacaoBimestralQueryHandler : IRequestHandler<ObterTipoAvaliacaoBimestralQuery, TipoAvaliacao>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;

        public ObterTipoAvaliacaoBimestralQueryHandler(IRepositorioCache repositorioCache, IRepositorioTipoAvaliacao repositorioTipoAvaliacao)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
        }
        public async Task<TipoAvaliacao> Handle(ObterTipoAvaliacaoBimestralQuery request, CancellationToken cancellationToken)
        {
            var tipoAvaliacao =  await repositorioCache.ObterAsync("tipo-avaliacao-bimestral", () => repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral());
            return tipoAvaliacao;
        }
    }
}
