using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoAvaliacaoPorCodigoQueryHandler : IRequestHandler<ObterTipoAvaliacaoPorCodigoQuery, long>
    {
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;

        public ObterTipoAvaliacaoPorCodigoQueryHandler(IRepositorioTipoAvaliacao repositorioTipoAvaliacao)
        {
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
        }

        public async Task<long> Handle(ObterTipoAvaliacaoPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioTipoAvaliacao.ObterIdPorCodigo(request.Codigo);
    }
}
