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
        
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        
        private readonly IMediator mediator;
        
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;

        public ObterPodeCadastrarAulaPorDataQueryHandler(IRepositorioEvento repositorioEvento, 
                                                         IRepositorioTipoCalendario repositorioTipoCalendario, 
                                                         IRepositorioFechamentoReabertura repositorioFechamentoReabertura, 
                                                         IMediator mediator,
            IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
        }
        public async Task<PodeCadastrarAulaPorDataRetornoDto> Handle(ObterPodeCadastrarAulaPorDataQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.Turma.CodigoTurma));

            // Periodo Escolar
            var periodoEscolar = await repositorioTipoCalendario.ObterPeriodoEscolarPorCalendarioEData(request.TipoCalendarioId, request.DataAula);
            
            if (periodoEscolar == null)
                return new PodeCadastrarAulaPorDataRetornoDto(false, "Não é possível cadastrar aula fora do periodo escolar");

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

            var temPeriodoAberto = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma, DateTimeExtension.HorarioBrasilia().Date, periodoEscolar.Bimestre);

            return temPeriodoAberto
                   ? new PodeCadastrarAulaPorDataRetornoDto(true)
                   : new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
        }
    }
}