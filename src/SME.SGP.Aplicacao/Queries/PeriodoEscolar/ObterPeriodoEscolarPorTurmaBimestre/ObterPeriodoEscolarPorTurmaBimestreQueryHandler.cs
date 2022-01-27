using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorTurmaBimestreQueryHandler : IRequestHandler<ObterPeriodoEscolarPorTurmaBimestreQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodoEscolarPorTurmaBimestreQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar;
        }

        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarPorTurmaBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPeriodoEscolarPorTurmaBimestre(request.Turma.CodigoTurma, request.Turma.ModalidadeTipoCalendario, request.Bimestre);
    }
}