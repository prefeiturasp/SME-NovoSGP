using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarComAberturaPorTurmaQueryHandler : IRequestHandler<ObterBimestreAtualComAberturaPorTurmaQuery, int>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodoEscolarComAberturaPorTurmaQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<int> Handle(ObterBimestreAtualComAberturaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterBimestreAtualComAberturaPorTurmaAsync(request.Turma.AnoLetivo, request.Turma.ModalidadeTipoCalendario, request.Turma.UeId, request.DataReferencia);
        }
    }
}
