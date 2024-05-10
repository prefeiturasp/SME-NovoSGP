using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorModalidadeDataFechamentoQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery, IEnumerable<PeriodoFechamentoBimestre>>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterPeriodosEscolaresPorModalidadeDataFechamentoQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }
        public async Task<IEnumerable<PeriodoFechamentoBimestre>> Handle(ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoFechamento.ObterPeriodosFechamentoBimestrePorDataFinal(request.ModalidadeTipoCalendario, request.DataFechamento);
        }
    }
}
