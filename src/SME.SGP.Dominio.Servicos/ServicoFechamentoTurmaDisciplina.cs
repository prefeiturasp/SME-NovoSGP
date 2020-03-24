using Microsoft.Extensions.Configuration;
using SME.Background.Core;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoTurmaDisciplina : IServicoFechamentoTurmaDisciplina
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;

        public ServicoFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre,
                                                IRepositorioDre repositorioDre,
                                                IRepositorioTurma repositorioTurma,
                                                IRepositorioUe repositorioUe,
                                                IServicoPeriodoFechamento servicoPeriodoFechamento,
                                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                IRepositorioTipoCalendario repositorioTipoCalendario,
                                                IRepositorioTipoAvaliacao repositorioTipoAvaliacao,
                                                IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
                                                IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
                                                IRepositorioParametrosSistema repositorioParametrosSistema,
                                                IRepositorioConceito repositorioConceito,
                                                IConsultasDisciplina consultasDisciplina,
                                                IServicoNotificacao servicoNotificacao,
                                                IServicoPendenciaFechamento servicoPendenciaFechamento,
                                                IServicoEOL servicoEOL,
                                                IServicoUsuario servicoUsuario,
                                                IUnitOfWork unitOfWork,
                                                IConfiguration configuration)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
        }

        public async Task<AuditoriaFechamentoTurmaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false)
        {
            var fechamentoTurma = MapearParaEntidade(id, entidadeDto);

            // Valida periodo de fechamento
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(fechamentoTurma.Turma.AnoLetivo
                                                                , fechamentoTurma.Turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio
                                                                , DateTime.Now.Month > 6 ? 2 : 1);

            var ue = fechamentoTurma.Turma.Ue;
            var periodoFechamento = await servicoPeriodoFechamento.ObterPorTipoCalendarioDreEUe(tipoCalendario.Id, ue.Dre, ue);
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == entidadeDto.Bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {entidadeDto.Bimestre}º Bimestre");

            // Valida Permissão do Professor na Turma/Disciplina
            VerificaSeProfessorPodePersistirTurma(servicoUsuario.ObterRf(), entidadeDto.TurmaId, periodoFechamentoBimestre.PeriodoEscolar.PeriodoFim);

            fechamentoTurma.PeriodoEscolarId = periodoFechamentoBimestre.PeriodoEscolarId;
            fechamentoTurma.PeriodoEscolar = periodoFechamentoBimestre.PeriodoEscolar;
            VarificaPercentualReprovacao(entidadeDto, fechamentoTurma.PeriodoEscolar);

            // Carrega notas alunos
            var notasConceitosBimestre = await MapearParaEntidade(id, entidadeDto.NotaConceitoAlunos);

            unitOfWork.IniciarTransacao();
            try
            {
                await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurma);
                foreach (var notaBimestre in notasConceitosBimestre)
                {
                    notaBimestre.FechamentoTurmaDisciplinaId = fechamentoTurma.Id;
                    repositorioNotaConceitoBimestre.Salvar(notaBimestre);
                }
                unitOfWork.PersistirTransacao();

                var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
                Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.GerarPendenciasFechamento(fechamentoTurma.DisciplinaId, fechamentoTurma.Turma, periodoFechamentoBimestre.PeriodoEscolar, fechamentoTurma, usuarioLogado, componenteSemNota));

                return (AuditoriaFechamentoTurmaDto)fechamentoTurma;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }

        private void VarificaPercentualReprovacao(FechamentoTurmaDisciplinaDto fechamentoTurmaDto, PeriodoEscolar periodoEscolar)
        {
            var percentualReprovacao = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.PercentualAlunosInsuficientes));
            var qtdReprovados = 0;

            // Verifica se lança nota ou conceito
            if (fechamentoTurmaDto.NotaConceitoAlunos.Any(a => a.ConceitoId > 0))
            {
                var conceitos = repositorioConceito.ObterPorData(periodoEscolar.PeriodoFim);
                qtdReprovados = fechamentoTurmaDto.NotaConceitoAlunos.Where(n => !conceitos.First(c => c.Id == n.ConceitoId).Aprovado).Count();
            }
            else
            {
                var mediaBimestre = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
                qtdReprovados = fechamentoTurmaDto.NotaConceitoAlunos.Where(c => c.Nota < mediaBimestre).Count();
            }

            // Mais de 50% reprovados
            if ((qtdReprovados > (fechamentoTurmaDto.NotaConceitoAlunos.Count() * percentualReprovacao / 100))
                && (string.IsNullOrEmpty(fechamentoTurmaDto.Justificativa)))
                throw new NegocioException($"Mais de {percentualReprovacao}% das notas/conceitos foi considerada insuficiente. Necessário incluir uma justificativa");
        }

        public async Task GerarPendenciasFechamento(long disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, FechamentoTurmaDisciplina fechamento, Usuario usuarioLogado, bool componenteSemNota = false)
        {
            var situacaoFechamento = SituacaoFechamento.ProcessadoComSucesso;

            int avaliacoesSemnota = 0;
            int alunosAbaixoMedia = 0;

            if (!componenteSemNota)
            {
                avaliacoesSemnota = servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(fechamento.Id, turma.CodigoTurma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
                alunosAbaixoMedia = servicoPendenciaFechamento.ValidarPercentualAlunosAbaixoDaMedia(fechamento);
            }
            
            var aulasReposicaoPendentes = servicoPendenciaFechamento.ValidarAulasReposicaoPendente(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            var aulasSemPlanoAula = servicoPendenciaFechamento.ValidarAulasSemPlanoAulaNaDataDoFechamento(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
            var aulasSemFrequencia = servicoPendenciaFechamento.ValidarAulasSemFrequenciaRegistrada(fechamento.Id, turma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);

            var quantidadePendencias = avaliacoesSemnota + aulasReposicaoPendentes + aulasSemPlanoAula + aulasSemFrequencia + alunosAbaixoMedia;
            if (quantidadePendencias > 0)
            {
                situacaoFechamento = SituacaoFechamento.ProcessadoComPendencias;
                GerarNotificacaoFechamento(fechamento, turma, quantidadePendencias, usuarioLogado);
            }

            fechamento.AtualizarSituacao(situacaoFechamento);
            await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamento);
        }

        public async Task Reprocessar(long fechamentoId)
        {
            var fechamento = repositorioFechamentoTurmaDisciplina.ObterPorId(fechamentoId);
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
            repositorioFechamentoTurmaDisciplina.Salvar(fechamento);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            Cliente.Executar<IServicoFechamentoTurmaDisciplina>(c => c.GerarPendenciasFechamento(fechamento.DisciplinaId, turma, periodoEscolar, fechamento, usuarioLogado, false));
        }

        private void GerarNotificacaoFechamento(FechamentoTurmaDisciplina fechamento, Turma turma, int quantidadePendencias, Usuario usuarioLogado)
        {
            var componentes = servicoEOL.ObterDisciplinasPorIds(new long[] { fechamento.DisciplinaId });
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
                Mensagem = $"O fechamento do {fechamento.PeriodoEscolar.Bimestre}º bimestre de {componentes.FirstOrDefault().Nome} da turma {turma.Nome} da {ue.Nome} ({dre.Nome}) gerou {quantidadePendencias} pendência(s). Clique <a href='{urlFrontEnd}'>aqui</a> para mais detalhes."
            };
            servicoNotificacao.Salvar(notificacao);
        }

        private void VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime data)
        {
            if (!servicoUsuario.PodePersistirTurma(codigoRf, turmaId, data).Result)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma e data.");
        }

        private async Task<IEnumerable<NotaConceitoBimestre>> MapearParaEntidade(long id, IEnumerable<NotaConceitoBimestreDto> notasConceitosAlunosDto)
        {
            var notasConceitosBimestre = new List<NotaConceitoBimestre>();

            if (id > 0)
            {
                // Edita as notas existentes
                notasConceitosBimestre = (await repositorioNotaConceitoBimestre.ObterPorFechamentoTurma(id)).ToList();

                foreach (var notaConceitoAlunoDto in notasConceitosAlunosDto)
                {
                    var notaConceitoBimestre = notasConceitosBimestre.FirstOrDefault(x => x.CodigoAluno == notaConceitoAlunoDto.CodigoAluno && x.DisciplinaId == notaConceitoAlunoDto.DisciplinaId);
                    if (notaConceitoBimestre != null)
                    {
                        notaConceitoBimestre.Nota = notaConceitoAlunoDto.Nota;

                        if (notaConceitoAlunoDto.ConceitoId > 0)
                            notaConceitoBimestre.ConceitoId = notaConceitoAlunoDto.ConceitoId;

                        if (notaConceitoAlunoDto.SinteseId > 0)
                            notaConceitoBimestre.SinteseId = notaConceitoAlunoDto.SinteseId;
                    }
                    else
                        notasConceitosBimestre.Add(MapearParaEntidade(notaConceitoAlunoDto));
                }
            }
            else
            {
                foreach (var notaConceitoAlunoDto in notasConceitosAlunosDto)
                {
                    notasConceitosBimestre.Add(MapearParaEntidade(notaConceitoAlunoDto));
                }
            }

            return notasConceitosBimestre;
        }

        private NotaConceitoBimestre MapearParaEntidade(NotaConceitoBimestreDto notaConceitoAlunoDto)
            => notaConceitoAlunoDto == null ? null :
              new NotaConceitoBimestre()
              {
                  CodigoAluno = notaConceitoAlunoDto.CodigoAluno,
                  DisciplinaId = notaConceitoAlunoDto.DisciplinaId,
                  Nota = notaConceitoAlunoDto.Nota,
                  ConceitoId = notaConceitoAlunoDto.ConceitoId,
                  SinteseId = notaConceitoAlunoDto.ConceitoId
              };

        private FechamentoTurmaDisciplina MapearParaEntidade(long id, FechamentoTurmaDisciplinaDto fechamentoDto)
        {
            var fechamento = new FechamentoTurmaDisciplina();
            if (id > 0)
                fechamento = repositorioFechamentoTurmaDisciplina.ObterPorId(id);

            fechamento.Situacao = SituacaoFechamento.EmProcessamento;
            fechamento.Turma = repositorioTurma.ObterTurmaComUeEDrePorId(fechamentoDto.TurmaId);
            fechamento.TurmaId = fechamento.Turma.Id;
            fechamento.DisciplinaId = fechamentoDto.DisciplinaId;
            fechamento.Justificativa = fechamentoDto.Justificativa;

            return fechamento;
        }

        private async Task<string> ValidaMinimoAvaliacoesBimestre(long tipoCalendarioId, string turmaId, long disciplinaId, int bimestre)
        {
            var validacoes = new StringBuilder();
            var tipoAvaliacaoBimestral = await repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral();

            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId });

            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Não foi possível localizar a disciplina no EOL.");

            if (disciplinasEOL.First().Regencia)
            {
                // Disciplinas Regencia de Classe
                disciplinasEOL = await consultasDisciplina.ObterDisciplinasParaPlanejamento(new FiltroDisciplinaPlanejamentoDto()
                {
                    CodigoTurma = long.Parse(turmaId),
                    CodigoDisciplina = disciplinaId,
                    Regencia = true
                });

                foreach (var disciplina in disciplinasEOL)
                {
                    var avaliacoes = await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaId, disciplina.CodigoComponenteCurricular.ToString(), bimestre);
                    if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        validacoes.AppendLine($"A disciplina [{disciplina.Nome}] não tem o número mínimo de avaliações bimestrais: Necessário {tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre}");
                }
            }
            else
            {
                var disciplinaEOL = disciplinasEOL.First();
                var avaliacoes = await repositorioAtividadeAvaliativaDisciplina.ObterAvaliacoesBimestrais(tipoCalendarioId, turmaId, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre);
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    validacoes.AppendLine($"A disciplina [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais: Necessário {tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre}");
            }

            return validacoes.ToString();
        }
    }
}