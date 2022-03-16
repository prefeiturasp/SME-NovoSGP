using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadeEPeriodosPassadosNoAnoQueryHandler : IRequestHandler<ObterModalidadeEPeriodosPassadosNoAnoQuery, IEnumerable<PeriodoEscolarModalidadeDto>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterModalidadeEPeriodosPassadosNoAnoQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<IEnumerable<PeriodoEscolarModalidadeDto>> Handle(ObterModalidadeEPeriodosPassadosNoAnoQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPeriodosPassadosNoAno(request.Data);
    }
}
