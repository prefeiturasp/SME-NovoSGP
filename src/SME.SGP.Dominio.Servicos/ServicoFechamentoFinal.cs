using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoFinal : IServicoFechamentoFinal
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;

        public ServicoFechamentoFinal(IRepositorioFechamentoFinal repositorioFechamentoFinal, IRepositorioPeriodoFechamento repositorioPeriodoFechamento,
            IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IRepositorioTurma repositorioTurma, IRepositorioTipoCalendario repositorioTipoCalendario, IRepositorioEvento repositorioEvento)
        {
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new ArgumentNullException(nameof(repositorioFechamentoFinal));
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task SalvarAsync(FechamentoFinal fechamentoFinal)
        {
            await repositorioFechamentoFinal.SalvarAsync(fechamentoFinal);

            //await VerificaFechamentoOuReabertura(fechamentoFinal.TurmaId);

            //fechamentoFinal.PodeSalvar()
        }

        public async Task VerificaFechamentoOuReabertura(Turma turma)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), turma.Semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi possível localizar o tipo de calendário.");

            var eventoFechamento = await repositorioEvento.EventosNosDiasETipo(DateTime.Today, DateTime.Today, TipoEvento.FechamentoBimestre, tipoCalendario.Id, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre);
            if (eventoFechamento == null)
                throw new NegocioException("Não foi possível localizar um fechamento de período ou reabertura para esta turma.");

            //var periodoFechamento = repositorioPeriodoFechamento.ObterPorFiltros(null, null, null, turmaId);

            //if (periodoFechamento == null)
            //{
            //    var turma = repositorioTurma.ObterPorId(turmaId);
            //    if (turma == null)
            //        throw new NegocioException("Não foi possível localizar a turma.");

            //    var fechamentoReabertura = await repositorioFechamentoReabertura.ObterPorTurma(turmaId);
            //    if (fechamentoReabertura == null)
            //        throw new NegocioException("Não foi possível localizar um fechamento de período ou reabertura para esta turma.");
            //}
        }
    }
}