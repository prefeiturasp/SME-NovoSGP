using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPodeCadastrarAulaPorDataQueryHandler : IRequestHandler<ObterPodeCadastrarAulaPorDataQuery, PodeCadastrarAulaPorDataRetornoDto>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IMediator mediator;

        public ObterPodeCadastrarAulaPorDataQueryHandler(IRepositorioEvento repositorioEvento,
                                                         IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                                         IRepositorioPeriodoFechamento repositorioPeriodoFechamento,
                                                         IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                                         IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                                         IMediator mediator)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
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

            var mesmoAnoLetivo = DateTime.Today.Year == request.DataAula.Year;

            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodoEscolar.Bimestre, mesmoAnoLetivo));

            return temPeriodoAberto
                   ? new PodeCadastrarAulaPorDataRetornoDto(true)
                   : new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
        }

    }
}