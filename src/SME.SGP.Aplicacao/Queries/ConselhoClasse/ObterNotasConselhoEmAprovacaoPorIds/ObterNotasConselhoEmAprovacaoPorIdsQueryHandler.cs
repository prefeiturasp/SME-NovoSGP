using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConselhoEmAprovacaoPorIdsQueryHandler : IRequestHandler<ObterNotasConselhoEmAprovacaoPorIdsQuery, IEnumerable<ConselhoClasseNotaAprovacaoDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;

        public ObterNotasConselhoEmAprovacaoPorIdsQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
        }

        public async Task<IEnumerable<ConselhoClasseNotaAprovacaoDto>> Handle(ObterNotasConselhoEmAprovacaoPorIdsQuery request, CancellationToken cancellationToken)
            => await repositorioConselhoClasseNota.ObterNotasConselhoEmAprovacaoPorIds(request.IdsConselhoClasseNota);
    }
}
