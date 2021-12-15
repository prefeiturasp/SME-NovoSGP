using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaQueryHandler : IRequestHandler<ObterPeriodoEscolarIdPorTurmaQuery, long>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IMediator mediator;

        public ObterPeriodoEscolarIdPorTurmaQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IMediator mediator)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(ObterPeriodoEscolarIdPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return await repositorioPeriodoEscolar.ObterPeriodoEscolarIdPorTurma(request.TurmaCodigo, turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
