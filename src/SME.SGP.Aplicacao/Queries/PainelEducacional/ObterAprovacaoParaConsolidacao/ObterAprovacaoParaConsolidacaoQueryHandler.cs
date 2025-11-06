using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao
{
    public class ObterAprovacaoParaConsolidacaoQueryHandler : IRequestHandler<ObterAprovacaoParaConsolidacaoQuery, IEnumerable<ConsolidacaoAprovacaoDto>>
    {
        private readonly IRepositorioPainelEducacionalAprovacao repositorio;

        public ObterAprovacaoParaConsolidacaoQueryHandler(IRepositorioPainelEducacionalAprovacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ConsolidacaoAprovacaoDto>> Handle(ObterAprovacaoParaConsolidacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterIndicadores(request.TurmaId);
        }
    }
}
