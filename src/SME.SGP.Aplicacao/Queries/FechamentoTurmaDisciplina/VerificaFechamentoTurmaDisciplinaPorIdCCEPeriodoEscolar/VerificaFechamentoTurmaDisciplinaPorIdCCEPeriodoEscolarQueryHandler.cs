using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQueryHandler : IRequestHandler<VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery, bool>
    {
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;

        public VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQueryHandler(IRepositorioFechamentoTurma repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<bool> Handle(VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.VerificaExistePorTurmaCCPeriodoEscolar(request.TurmaId, request.ComponenteCurricularId, request.PeriodoEscolarId);
        }

    }
}
