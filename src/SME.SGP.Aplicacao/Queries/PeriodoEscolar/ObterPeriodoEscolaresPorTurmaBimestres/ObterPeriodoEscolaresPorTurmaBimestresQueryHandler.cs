using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolaresPorTurmaBimestresQueryHandler : IRequestHandler<ObterPeriodoEscolarPorTurmaBimestreQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodoEscolaresPorTurmaBimestresQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar;
        }

        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarPorTurmaBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPeriodoEscolarPorTurmaBimestre(request.Turma.CodigoTurma, request.Turma.ModalidadeTipoCalendario, request.Bimestre);
    }
}
