using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasAulaDiarioClasseFechamentoCommandHandler : IRequestHandler<ExcluirPendenciasAulaDiarioClasseFechamentoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendencia;

        public ExcluirPendenciasAulaDiarioClasseFechamentoCommandHandler(IMediator mediator, IRepositorioPendenciaAulaConsulta repositorioPendencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<bool> Handle(ExcluirPendenciasAulaDiarioClasseFechamentoCommand request, CancellationToken cancellationToken)
        {
            var pendencias = await repositorioPendencia.ObterPendenciasAulaDiarioClassePorTurmaDisciplinaPeriodo(request.TurmaCodigo, request.DisciplinaId, request.PeriodoInicio, request.PeriodoFim);
            if (pendencias == null || !pendencias.Any())
                return false;
            return (await mediator.Send(new ExcluirPendenciasPorIdsCommand() { PendenciasIds = pendencias.ToArray() }));
        }
    }
}
