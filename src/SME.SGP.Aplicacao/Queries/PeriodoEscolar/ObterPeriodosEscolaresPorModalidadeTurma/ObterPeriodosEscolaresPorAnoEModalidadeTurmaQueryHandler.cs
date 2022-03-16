using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorAnoEModalidadeTurmaQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery, IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodosEscolaresPorAnoEModalidadeTurmaQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterPorAnoLetivoEModalidadeTurma(request.AnoLetivo, request.ModalidadeTurma.ObterModalidadeTipoCalendario(), request.Semestre);
        }
    }
}
