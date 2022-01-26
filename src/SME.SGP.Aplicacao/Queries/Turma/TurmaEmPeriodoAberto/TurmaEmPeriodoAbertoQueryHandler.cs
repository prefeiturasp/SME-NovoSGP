using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoAbertoQueryHandler : IRequestHandler<TurmaEmPeriodoAbertoQuery, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public TurmaEmPeriodoAbertoQueryHandler(IMediator mediator,
                                                IRepositorioTipoCalendario repositorioTipoCalendario,
                                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<bool> Handle(TurmaEmPeriodoAbertoQuery request, CancellationToken cancellationToken)
        {
            long tipoCalendarioId;

            if (request.TipoCalendarioId == 0)
                tipoCalendarioId = await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.Turma.AnoLetivo
                                        , request.Turma.ModalidadeTipoCalendario
                                        , request.Turma.Semestre);
            else tipoCalendarioId = request.TipoCalendarioId;

            if (tipoCalendarioId == 0)
                throw new NegocioException($"Tipo de calendário para turma {request.Turma.CodigoTurma} não localizado!");

            var periodoEmAberto = await repositorioPeriodoEscolar.PeriodoEmAbertoAsync(tipoCalendarioId, request.DataReferencia, request.Bimestre, request.EhAnoLetivo);

            return periodoEmAberto || await TurmaEmPeriodoDeFechamento(request.Turma, tipoCalendarioId, request.DataReferencia, request.Bimestre);
        }

        private async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, long tipoCalendarioId, DateTime dataReferencia, int bimestre)
            => await mediator.Send(new TurmaEmPeriodoFechamentoQuery(turma, bimestre, dataReferencia, tipoCalendarioId));
    }
}
