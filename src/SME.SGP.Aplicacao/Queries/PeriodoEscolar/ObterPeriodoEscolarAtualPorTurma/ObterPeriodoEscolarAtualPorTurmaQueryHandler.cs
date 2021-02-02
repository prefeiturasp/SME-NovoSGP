using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarAtualPorTurmaQueryHandler : IRequestHandler<ObterPeriodoEscolarAtualPorTurmaQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodoEscolarAtualPorTurmaQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar;
        }

        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarAtualPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPeriodoEscolarAtualPorTurmaIdAsync(request.Turma.CodigoTurma, request.Turma.ModalidadeTipoCalendario, request.DataReferencia);
    }
}