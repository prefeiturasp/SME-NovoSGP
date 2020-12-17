using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorModalidadeAnoEDataFinalQueryHandler : IRequestHandler<ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodoEscolarPorModalidadeAnoEDataFinalQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPorModalidadeAnoEDataFinal(request.Modalidade, request.Ano, request.DataFim);
    }
}
