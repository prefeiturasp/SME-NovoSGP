using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterConsolidacaoPlanosAEE
{
    public class ObterConsolidacaoPlanoAEEQueryHandler : IRequestHandler<ObterConsolidacaoPlanoAEEQuery, IEnumerable<ConsolidacaoPlanoAEEDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterConsolidacaoPlanoAEEQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ConsolidacaoPlanoAEEDto>> Handle(ObterConsolidacaoPlanoAEEQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterPlanosConsolidarPainelEducacional();
            return registros;
        }
    }
}
