using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoVigentePorTurmaDataBimestreQueryHandler : IRequestHandler<ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery, PeriodoFechamentoVigenteDto>
    {
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;

        public ObterPeriodoFechamentoVigentePorTurmaDataBimestreQueryHandler(IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
        }
        public async Task<PeriodoFechamentoVigenteDto> Handle(ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery request, CancellationToken cancellationToken)
              => await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamentoVigente(request.Turma, request.DataReferencia, request.Bimestre);
    }
}
