using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPodeCadastrarAulaPorDataQueryHandler : IRequestHandler<ObterPodeCadastrarAulaPorDataQuery, PodeCadastrarAulaPorDataRetornoDto>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IMediator mediator;

        public ObterPodeCadastrarAulaPorDataQueryHandler(IRepositorioEvento repositorioEvento, IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioPeriodoFechamento repositorioPeriodoFechamento, IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IMediator mediator)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<PodeCadastrarAulaPorDataRetornoDto> Handle(ObterPodeCadastrarAulaPorDataQuery request, CancellationToken cancellationToken)
        {
            var hoje = DateTime.Today.Date;
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.Turma.CodigoTurma));
            var somenteAulaReposicao = false;

            // Periodo Escolar
            var periodoEscolar = await repositorioTipoCalendario.ObterPeriodoEscolarPorCalendarioEData(request.TipoCalendarioId, request.DataAula);
            if (periodoEscolar == null)
            {
                var eventoReposicaoAulaNoDia = await repositorioEvento
                    .EventosNosDiasETipo(request.DataAula, request.DataAula, TipoEvento.ReposicaoDoDia, request.TipoCalendarioId, turma.Ue.CodigoUe, string.Empty);

                var eventoReposicaoDeAula = await repositorioEvento
                    .EventosNosDiasETipo(request.DataAula, request.DataAula, TipoEvento.ReposicaoDeAula, request.TipoCalendarioId, turma.Ue.CodigoUe, string.Empty);

                if (eventoReposicaoAulaNoDia == null && eventoReposicaoDeAula == null)
                    return new PodeCadastrarAulaPorDataRetornoDto(false, "Não é possível cadastrar aula fora do periodo escolar");

                somenteAulaReposicao = true;
            }

            // Domingo
            if (request.DataAula.FimDeSemana())
            {
                // Evento Letivo
                var temEventoLetivoNoDia = await repositorioEvento.EhEventoLetivoPorTipoDeCalendarioDataDreUe(request.TipoCalendarioId, request.DataAula, request.DreCodigo, request.UeCodigo);
                if (!temEventoLetivoNoDia)
                    return new PodeCadastrarAulaPorDataRetornoDto(false, "Não é possível cadastrar aula no final de semana");
            }

            // Evento não letivo
            var temEventoNaoLetivoNoDia = await repositorioEvento.EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(request.TipoCalendarioId, request.DataAula, request.DreCodigo, request.UeCodigo);
            if (temEventoNaoLetivoNoDia)
                return new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois existe um evento de dia não letivo");

            if (request.DataAula.Year == hoje.Year)
            {
                if (request.DataAula.Date <= hoje)
                {
                    // Consultar fechamento só se não for data no bimestre corrente
                    var periodoEscolarAtual = await repositorioTipoCalendario.ObterPeriodoEscolarPorCalendarioEData(request.TipoCalendarioId, hoje);
                    if (periodoEscolarAtual == null || periodoEscolar.Id != periodoEscolarAtual.Id)
                    {
                        if (periodoEscolar == null)
                        {
                            var periodos = (await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre))).OrderBy(p => p.Bimestre);
                            periodoEscolar = request.DataAula.Date < periodos.First().PeriodoInicio.Date ? periodos.First() : periodos.Last();
                        }

                        var periodoFechamento = await repositorioPeriodoFechamento.ObterPeriodoPorUeDataBimestreAsync(request.Turma.UeId, hoje, periodoEscolar.Bimestre);
                        var fechamentoPeriodoDataAula = periodoFechamento?.FechamentosBimestre.Where(x => x.PeriodoEscolarId == periodoEscolar.Id).ToList();
                        if (fechamentoPeriodoDataAula != null && fechamentoPeriodoDataAula.Any())
                        {
                            periodoFechamento.FechamentosBimestre = fechamentoPeriodoDataAula;
                            if (periodoFechamento.ExisteFechamentoEmAberto(hoje))
                                return new PodeCadastrarAulaPorDataRetornoDto(true);
                        }
                        else
                        {
                            FechamentoReabertura periodoFechamentoReabertura = await ObterPeriodoFechamentoReabertura(request.TipoCalendarioId, request.Turma.UeId, hoje);
                            var mesmoAnoLetivo = DateTime.Today.Year == request.DataAula.Year;
                            bool TemPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodoEscolar.Bimestre, mesmoAnoLetivo));

                            return (periodoFechamentoReabertura != null && TemPeriodoAberto) ?
                                new PodeCadastrarAulaPorDataRetornoDto(true, somenteReposicao: somenteAulaReposicao) :
                                new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
                        }
                    }
                }
                else
                    return new PodeCadastrarAulaPorDataRetornoDto(true, somenteReposicao: somenteAulaReposicao);
            }
            else
            {
                FechamentoReabertura periodoFechamentoReabertura = await ObterPeriodoFechamentoReabertura(request.TipoCalendarioId, request.Turma.UeId, hoje);
                return periodoFechamentoReabertura != null ?
                    new PodeCadastrarAulaPorDataRetornoDto(true, somenteReposicao: somenteAulaReposicao) :
                    new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
            }

            return new PodeCadastrarAulaPorDataRetornoDto(true, somenteReposicao: somenteAulaReposicao);
        }

        private async Task<FechamentoReabertura> ObterPeriodoFechamentoReabertura(long tipoCalendarioId, long ueId, DateTime hoje)
        {
            return await repositorioFechamentoReabertura.ObterPorDataTurmaCalendarioAsync(ueId, hoje, tipoCalendarioId);
        }
    }
}
