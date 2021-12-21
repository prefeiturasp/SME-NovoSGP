using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualPorTurmaIdQueryHandler : IRequestHandler<ObterBimestreAtualPorTurmaIdQuery, int>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterBimestreAtualPorTurmaIdQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<int> Handle(ObterBimestreAtualPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterBimestreAtualPorTurmaIdAsync(request.Turma.Id, request.Turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
