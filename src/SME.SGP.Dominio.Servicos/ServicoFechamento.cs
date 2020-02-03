using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamento : IServicoFechamento
    {
        private readonly IRepositorioFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento,
                                 IRepositorioTurma repositorioTurma,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                 IServicoPendenciaFechamento servicoPendenciaFechamento)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
        }

        public void RealizarFechamento(string codigoTurma, string disciplinaId, long periodoEscolarId)
        {
            var turma = repositorioTurma.ObterPorId(codigoTurma);
            if (turma == null)
            {
                throw new NegocioException("Turma não encontrada.");
            }

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(periodoEscolarId);
            if (periodoEscolar == null)
            {
                throw new NegocioException("Período escolar não encontrado.");
            }

            if (!turma.MesmaModalidadePeriodoEscolar(periodoEscolar.TipoCalendario.Modalidade))
            {
                throw new NegocioException("Essa turma não pertence ao tipo de calendário informado.");
            }

            var fechamento = new Fechamento(turma.Id, disciplinaId, periodoEscolar.Id);
            fechamento.Id = repositorioFechamento.Salvar(fechamento);

            GerarPendenciasFechamento(disciplinaId, turma, periodoEscolar, fechamento);

            if (fechamento.Situacao != SituacaoFechamento.EmProcessamento)
                repositorioFechamento.Salvar(fechamento);
        }

        private void GerarPendenciasFechamento(string disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, Fechamento fechamento)
        {
            var situacaoFechamento = SituacaoFechamento.ProcessadoComSucesso;

            if (servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(fechamento.Id, turma.CodigoTurma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim))
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            if (servicoPendenciaFechamento.ValidarAulasReposicaoPendente(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim))
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            if (servicoPendenciaFechamento.ValidarAulasSemPlanoAulaNaDataDoFechamento(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim))
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            if (servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim))
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            fechamento.AtualizarSituacao(situacaoFechamento);
        }
    }
}