using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQueryHandler : IRequestHandler<ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQuery, RetornoConsolidacaoExistenteDto>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;

        public ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQueryHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
        }
        public async Task<RetornoConsolidacaoExistenteDto> Handle(ObterConsolidacaoExistentePorTurmaIdAnoLetivoTipoPeriodoMesQuery request, CancellationToken cancellationToken)
            => await repositorioConsolidacaoFrequenciaTurma.ObterConsolidacaoDashboardPorTurmaAnoTipoPeriodoMes(request.TurmaId, request.AnoLetivo, request.TipoPeriodo, request.DataAula, request.Mes, request.DataInicioSemana, request.DataFimSemana); 
    }
}
