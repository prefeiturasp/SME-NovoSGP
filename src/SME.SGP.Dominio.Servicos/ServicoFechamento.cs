using Microsoft.Extensions.Configuration;
using SME.Background.Core;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamento : IServicoFechamento
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento,
                                 IRepositorioTurma repositorioTurma,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                 IServicoPendenciaFechamento servicoPendenciaFechamento,
                                 IRepositorioTipoCalendario repositorioTipoCalendario,
                                 IServicoUsuario servicoUsuario,
                                 IServicoEOL servicoEOL,
                                 IRepositorioUe repositorioUe,
                                 IRepositorioDre repositorioDre,
                                 IConfiguration configuration,
                                 IServicoNotificacao servicoNotificacao)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioUe = repositorioUe;
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
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

            //TODO VALIDAR PERCENTUAL DE ALUNOS ABAIXO DA MEDIA QUANDO HISTORIA 9269 ESTIVER CONCLUIDA

            fechamento.AtualizarSituacao(situacaoFechamento);
            var quantidadePendencias = avaliacoesSemnota + aulasReposicaoPendentes + aulasSemPlanoAula + aulasSemFrequencia;
            if (situacaoFechamento != SituacaoFechamento.ProcessadoComSucesso && quantidadePendencias > 0)
            {
                GerarNotificacaoFechamento(fechamento, turma, quantidadePendencias, usuarioLogado);
            }
            repositorioFechamento.Salvar(fechamento);
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
            fechamento.AdicionarPeriodoEscolar(periodoEscolar);
            fechamento.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
            repositorioFechamento.Salvar(fechamento);
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            Cliente.Executar<IServicoFechamento>(c => c.GerarPendenciasFechamento(fechamento.DisciplinaId, turma, periodoEscolar, fechamento, usuarioLogado));
        }

        private void GerarNotificacaoFechamento(Fechamento fechamento, Turma turma, int quantidadePendencias, Usuario usuarioLogado)
        {
            var componentes = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(fechamento.DisciplinaId) });
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Componente curricular não encontrado.");
            }
            var ue = repositorioUe.ObterPorId(turma.UeId);
            if (ue == null)
                throw new NegocioException("UE não encontrada.");

            var dre = repositorioDre.ObterPorId(ue.DreId);
            if (dre == null)
                throw new NegocioException("DRE não encontrada.");

            var urlFrontEnd = configuration["UrlFrontEnd"];
            if (string.IsNullOrWhiteSpace(urlFrontEnd))
                throw new NegocioException("Url do frontend não encontrada.");

            var notificacao = new Notificacao()
            {
                UsuarioId = usuarioLogado.Id,
                Ano = DateTime.Now.Year,
                Categoria = NotificacaoCategoria.Aviso,
                Titulo = $"Pendência no fechamento da turma {turma.Nome}",
                Tipo = NotificacaoTipo.Fechamento,
                Mensagem = $"O fechamento do {fechamento.PeriodoEscolar.Bimestre}Nº bimestre de {componentes.FirstOrDefault().Nome} da turma {turma.Nome} da {ue.Nome} ({dre.Nome}) gerou {quantidadePendencias} pendência(s). Clique <a href='{urlFrontEnd}'>aqui</a> para mais detalhes."
            };
            servicoNotificacao.Salvar(notificacao);
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