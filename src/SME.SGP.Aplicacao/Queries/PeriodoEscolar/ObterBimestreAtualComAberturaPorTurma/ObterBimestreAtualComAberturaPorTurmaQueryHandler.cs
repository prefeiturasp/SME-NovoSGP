using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarComAberturaPorTurmaQueryHandler : IRequestHandler<ObterBimestreAtualComAberturaPorTurmaQuery, int>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodoEscolarComAberturaPorTurmaQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<int> Handle(ObterBimestreAtualComAberturaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterBimestreAtualComAberturaPorAnoModalidade(request.Turma.AnoLetivo, request.Turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
