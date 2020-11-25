using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorModalidadeDataFechamentoQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery, IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodosEscolaresPorModalidadeDataFechamentoQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterPorModalidadeDataFechamento(request.ModalidadeTipoCalendario, request.DataFechamento);
        }
    }
}
