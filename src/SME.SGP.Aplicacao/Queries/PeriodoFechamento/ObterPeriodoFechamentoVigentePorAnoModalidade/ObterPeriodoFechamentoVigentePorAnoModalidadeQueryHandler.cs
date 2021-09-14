using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoVigentePorAnoModalidadeQueryHandler : IRequestHandler<ObterPeriodoFechamentoVigentePorAnoModalidadeQuery, PeriodoFechamentoVigenteDto>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterPeriodoFechamentoVigentePorAnoModalidadeQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task<PeriodoFechamentoVigenteDto> Handle(ObterPeriodoFechamentoVigentePorAnoModalidadeQuery request, CancellationToken cancellationToken)
        {
            var modalidade = request.ModalidadeTipoCalendario ?? Dominio.ModalidadeTipoCalendario.FundamentalMedio;
            return await repositorioPeriodoFechamento.ObterPeriodoVigentePorAnoModalidade(request.AnoLetivo, (int)modalidade);
        }
    }
}
