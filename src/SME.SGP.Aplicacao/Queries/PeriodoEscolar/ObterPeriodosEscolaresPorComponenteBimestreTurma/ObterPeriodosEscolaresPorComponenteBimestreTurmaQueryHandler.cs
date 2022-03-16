using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorComponenteBimestreTurmaQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery, IEnumerable<PeriodoEscolarVerificaRegenciaDto>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodosEscolaresPorComponenteBimestreTurmaQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar;
        }

        public async Task<IEnumerable<PeriodoEscolarVerificaRegenciaDto>> Handle(ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery request, CancellationToken cancellationToken)
         => await repositorioPeriodoEscolar.ObterPeriodoEscolaresPorTurmaComponenteBimestre(request.TurmaCodigo, request.ComponenteCodigo, request.Bimestre,request.AulaCj);
    }
}
