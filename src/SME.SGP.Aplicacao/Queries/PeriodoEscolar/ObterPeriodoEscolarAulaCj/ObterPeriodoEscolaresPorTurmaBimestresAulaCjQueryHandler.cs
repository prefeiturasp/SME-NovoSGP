using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolaresPorTurmaBimestresAulaCjQueryHandler : IRequestHandler<ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery, PeriodoEscolarBimestreDto>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        public ObterPeriodoEscolaresPorTurmaBimestresAulaCjQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar;
        }
        public async Task<PeriodoEscolarBimestreDto> Handle(ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery request, CancellationToken cancellationToken)
         => await repositorioPeriodoEscolar.ObterPeriodoEscolarPorTurmaBimestreAulaCj(request.Turma.CodigoTurma, request.Turma.ModalidadeTipoCalendario, request.Bimestre, request.AulaCj);
    }
}
