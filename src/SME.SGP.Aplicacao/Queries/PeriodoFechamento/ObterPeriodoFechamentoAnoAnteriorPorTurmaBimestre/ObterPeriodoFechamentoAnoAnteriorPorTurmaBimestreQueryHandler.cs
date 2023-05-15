using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQueryHandler : IRequestHandler<ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQuery, PeriodoFechamentoVigenteDto>
    {
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;

        public ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQueryHandler(IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
        }
        public async Task<PeriodoFechamentoVigenteDto> Handle(ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQuery request, CancellationToken cancellationToken)
            => await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamentoAnoAnterior(request.Turma, request.Bimestre);
    }
}
