using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.DiarioBordo.ObterDiariosDeBordoPorPeriodo
{
    public class ObterDiariosDeBordoPorPeriodoQueryHandler : ConsultasBase, IRequestHandler<ObterDiariosDeBordoPorPeriodoQuery, PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;

        public ObterDiariosDeBordoPorPeriodoQueryHandler(IContextoAplicacao contextoAplicacao,
                                                         IRepositorioDiarioBordo repositorioDiarioBordo,
                                                         IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Handle(ObterDiariosDeBordoPorPeriodoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));

            return await repositorioDiarioBordo.ObterDiariosBordoPorPeriodoPaginado(request.TurmaCodigo, turma.AnoLetivo, request.ComponenteCurricularCodigo, request.PeriodoInicio, request.PeriodoFim, Paginacao);
        } 
    }
}
