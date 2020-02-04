using SME.Background.Core;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamento : IServicoFechamento
    {
        private readonly IRepositorioFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento,
                                 IRepositorioTurma repositorioTurma,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                 IServicoPendenciaFechamento servicoPendenciaFechamento,
                                 IRepositorioTipoCalendario repositorioTipoCalendario,
                                 IServicoUsuario servicoUsuario)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public void GerarPendenciasFechamento(string disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, Fechamento fechamento, Usuario usuarioLogado)
        {
            var situacaoFechamento = SituacaoFechamento.ProcessadoComSucesso;

            var avaliacoesSemnota = servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(fechamento.Id, turma.CodigoTurma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            if (avaliacoesSemnota > 0)
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            var aulasReposicaoPendentes = servicoPendenciaFechamento.ValidarAulasReposicaoPendente(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            if (aulasReposicaoPendentes > 0)
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            var aulasSemPlanoAula = servicoPendenciaFechamento.ValidarAulasSemPlanoAulaNaDataDoFechamento(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            if (aulasSemPlanoAula > 0)
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            var aulasSemFrequencia = servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            if (aulasSemFrequencia > 0)
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;

            fechamento.AtualizarSituacao(situacaoFechamento);
            if (fechamento.Situacao != SituacaoFechamento.EmProcessamento)
            {
                GerarNotificacaoFechamento(fechamento, avaliacoesSemnota + aulasReposicaoPendentes + aulasSemPlanoAula + aulasSemFrequencia, usuarioLogado);
                repositorioFechamento.Salvar(fechamento);
            }
        }

        public void RealizarFechamento(string codigoTurma, string disciplinaId, long periodoEscolarId, Usuario usuarioLogado)
        {
            var (turma, periodoEscolar) = ValidarTurmaEPeriodoEscolar(codigoTurma, periodoEscolarId);
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(periodoEscolar.TipoCalendarioId);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Tipo de calendário não encontrado.");
            }

            if (!turma.MesmaModalidadePeriodoEscolar(tipoCalendario.Modalidade))
            {
                throw new NegocioException("Essa turma não pertence ao tipo de calendário informado.");
            }
            Fechamento fechamento = repositorioFechamento.ObterPorTurmaDisciplinaPeriodo(turma.Id, disciplinaId, periodoEscolar.Id);
            if (fechamento == null)
            {
                fechamento = new Fechamento(turma.Id, disciplinaId, periodoEscolar.Id);
            }
            fechamento.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            fechamento.Id = repositorioFechamento.Salvar(fechamento);

            Cliente.Executar<IServicoFechamento>(c => c.GerarPendenciasFechamento(fechamento.DisciplinaId, turma, periodoEscolar, fechamento, usuarioLogado));
        }

        public async Task Reprocessar(long fechamentoId)
        {
            var fechamento = repositorioFechamento.ObterPorId(fechamentoId);
            if (fechamento == null)
            {
                throw new NegocioException("Fechamento ainda não realizado para essa turma.");
            }
            var turma = repositorioTurma.ObterPorId(fechamento.TurmaId);
            if (turma == null)
            {
                throw new NegocioException("Turma não encontrada.");
            }

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(fechamento.PeriodoEscolarId);
            if (periodoEscolar == null)
            {
                throw new NegocioException("Período escolar não encontrado.");
            }
            fechamento.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            repositorioFechamento.Salvar(fechamento);
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            Cliente.Executar<IServicoFechamento>(c => c.GerarPendenciasFechamento(fechamento.DisciplinaId, turma, periodoEscolar, fechamento, usuarioLogado));
        }

        private void GerarNotificacaoFechamento(Fechamento fechamento, int quantidadePendencias, Usuario usuarioLogado)
        {
            if (fechamento.Situacao == SituacaoFechamento.ProcessadoComPendencias && quantidadePendencias > 0)
            {
                var notificacao = new Notificacao()
                {
                };
            }
        }

        private (Turma, PeriodoEscolar) ValidarTurmaEPeriodoEscolar(string codigoTurma, long periodoEscolarId)
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
            return (turma, periodoEscolar);
        }
    }
}