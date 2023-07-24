using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQueryHandler : IRequestHandler<ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery, ConsolidacaoDashBoardFrequencia>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;

        public ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQueryHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
        }

        public async Task<ConsolidacaoDashBoardFrequencia> Handle(ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery request, CancellationToken cancellationToken)
            => await repositorioConsolidacaoFrequenciaTurma.ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipo(request.TurmaId,
                request.DataAula,request.Modalidade,request.AnoLetivo, request.DreId, request.UeId, request.Tipo);
    }
}
