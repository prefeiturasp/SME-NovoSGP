using MediatR;
using SME.SGP.Dominio;
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
            var fechamento = await repositorioFechamentoTurma.ObterIdEPeriodoPorTurmaBimestre(request.TurmaId, request.Bimestre > 0 ? (int?)request.Bimestre : null);

            var periodoEscolarId = fechamento.EhNulo() ?
                await ObterPeriodoEscolarId(request.TurmaId, request.Bimestre) :
                fechamento.PeriodoEscolarId;

            if (fechamento.EhNulo())
                fechamento = new FechamentoTurmaPeriodoEscolarDto() { PeriodoEscolarId = periodoEscolarId };

            if (periodoEscolarId.HasValue)
                fechamento.PossuiAvaliacao = await mediator.Send(new TurmaPossuiAvaliacaoNoPeriodoQuery(request.TurmaId, periodoEscolarId.Value));            

            return fechamento;
        }

        private async Task<long?> ObterPeriodoEscolarId(long turmaId, int bimestre)
        {
            if (bimestre == 0)
                return 0;

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            if (turma.EhNulo()) return null;
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));

            return periodoEscolar.Id;
        }
    }
}
