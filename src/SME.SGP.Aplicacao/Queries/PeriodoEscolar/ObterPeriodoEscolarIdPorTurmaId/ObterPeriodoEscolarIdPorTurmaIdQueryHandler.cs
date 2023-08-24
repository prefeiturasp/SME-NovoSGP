using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaIdQueryHandler : IRequestHandler<ObterPeriodoEscolarIdPorTurmaIdQuery, long>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodoEscolarIdPorTurmaIdQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<long> Handle(ObterPeriodoEscolarIdPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterPeriodoEscolarIdPorTurmaId(request.Turma.Id, request.Turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
