using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaComPeriodoEscolarQueryHandler : IRequestHandler<ObterFechamentoTurmaComPeriodoEscolarQuery, FechamentoTurmaPeriodoEscolarDto>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;
        private readonly IMediator mediator;

        public ObterFechamentoTurmaComPeriodoEscolarQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma, IMediator mediator)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FechamentoTurmaPeriodoEscolarDto> Handle(ObterFechamentoTurmaComPeriodoEscolarQuery request, CancellationToken cancellationToken)
        {
            var fechamento = await repositorioFechamentoTurma.ObterIdEPeriodoPorTurmaBimestre(request.TurmaId, request.Bimestre > 0 ? request.Bimestre : null);

            if (fechamento == null)
                return null;

            if (fechamento.PeriodoEscolarId.HasValue)
                fechamento.PossuiAvaliacao = await mediator.Send(new TurmaPossuiAvaliacaoNoPeriodoQuery(request.TurmaId, fechamento.PeriodoEscolarId.Value));

            return fechamento;
        }
    }
}
