using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PeriodoEstaEmAbertoPAPQueryHandler : IRequestHandler<PeriodoEstaEmAbertoPAPQuery, bool>
    {
        private readonly IRepositorioPeriodoRelatorioPAP repositorio;
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IMediator mediator;

        public PeriodoEstaEmAbertoPAPQueryHandler(IRepositorioPeriodoRelatorioPAP repositorio,
                                                  IRepositorioEventoFechamentoConsulta repositorioEventoFechamento,
                                                  IMediator mediator,
                                                  IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<bool> Handle(PeriodoEstaEmAbertoPAPQuery request, CancellationToken cancellationToken)
        {
            var periodoEmAberto = await repositorio.PeriodoEmAberto(request.PeriodoRelatorioId, DateTimeExtension.HorarioBrasilia().Date);

            return periodoEmAberto || await TurmaEmPeriodoDeFechamentoReabertura(request.Turma, request.PeriodoRelatorioId);
        }

        private async Task<bool> TurmaEmPeriodoDeFechamentoReabertura(Turma turma, long periodoRelatorioId)
        {
            var periodoRelatorio = await this.repositorio.ObterComPeriodosEscolares(periodoRelatorioId);
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(turma.ModalidadeTipoCalendario, turma.AnoLetivo, turma.Semestre));

            return await TurmaEmPeriodoDeFechamento(turma, periodoRelatorio, tipoCalendarioId) || 
                await UeEmReaberturaDeFechamento(tipoCalendarioId, turma, periodoRelatorio);
        }

        private async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, PeriodoRelatorioPAP periodoRelatorio, long tipoCalendarioId)
        {
            var periodosFechamento = await this.repositorioEventoFechamento.UeEmFechamentoVigente(DateTimeExtension.HorarioBrasilia().Date, tipoCalendarioId, turma.EhTurmaInfantil, 0);
        
            if (periodosFechamento != null)
            {
                return periodoRelatorio.PeriodosEscolaresRelatorio.Any(periodo => periodo.Id == periodosFechamento.PeriodoEscolarId);
            }
            
            return false;
        }

        private async Task<bool> UeEmReaberturaDeFechamento(long tipoCalendarioId, Turma turma, PeriodoRelatorioPAP periodoRelatorio)
        {
            const int PRIMEIRO_SEMESTRE = 1;
            const int SEGUNDO_BIMESTRE = 2;
            const int QUARTO_BIMESTRE = 4;

            int bimestre = periodoRelatorio.Periodo; 

            if (periodoRelatorio.Configuracao.EhSemestre)
                bimestre = periodoRelatorio.Periodo == PRIMEIRO_SEMESTRE ? SEGUNDO_BIMESTRE : QUARTO_BIMESTRE;

            var reaberturaPeriodo = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                                                            bimestre,
                                                            DateTimeExtension.HorarioBrasilia().Date,
                                                            tipoCalendarioId,
                                                            turma.Ue.Dre.CodigoDre,
                                                            turma.Ue.CodigoUe);
            return reaberturaPeriodo != null;
        }
    }
}
