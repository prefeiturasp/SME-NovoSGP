using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQueryHandler : IRequestHandler<ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery, long>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IMediator mediator;

        public ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IMediator mediator)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(ObterPeriodoEscolarIdPorTurmaBimestreAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo), cancellationToken);
            
            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            return await repositorioPeriodoEscolar.ObterPeriodoEscolarIdPorTurmaBimestre(request.TurmaCodigo,
                turma.ModalidadeTipoCalendario, request.Bimestre, request.AnoLetivo, turma.Semestre);
        }
    }
}
