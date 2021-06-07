using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoCCQueryHandler : IRequestHandler<ObterFechamentosPorTurmaPeriodoCCQuery, IEnumerable<FechamentoTurmaDisciplina>>
    {
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;

        public ObterFechamentoPorTurmaPeriodoCCQueryHandler(IRepositorioFechamentoTurma repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> Handle(ObterFechamentosPorTurmaPeriodoCCQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterPorTurmaPeriodoCCAsync(request.TurmaId, request.PeriodoEscolarId, request.ComponenteCurricularId);            
        }
    }
}
