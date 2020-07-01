using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoWorkflowAprovacao : IServicoWorkflowAprovacao
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel;

        public ServicoWorkflowAprovacao(IRepositorioNotificacao repositorioNotificacao,
                                        IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao,
                                        IServicoEol servicoEOL,
                                        IServicoUsuario servicoUsuario,
                                        IServicoNotificacao servicoNotificacao,
                                        IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel,
                                        IRepositorioEvento repositorioEvento,
                                        IConfiguration configuration,
                                        IRepositorioAula repositorioAula,
                                        IRepositorioUe repositorioUe,
                                        IRepositorioTurma repositorioTurma,
                                        IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
                                        IUnitOfWork unitOfWork,
                                        IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                        IRepositorioFechamentoNota repositorioFechamentoNota,
                                        IRepositorioUsuario repositorioUsuario,
                                        IRepositorioPendencia repositorioPendencia,
                                        IRepositorioEventoTipo repositorioEventoTipo)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioWorkflowAprovacaoNivelNotificacao = repositorioWorkflowAprovacaoNivelNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelNotificacao));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoNotificacao = servicoNotificacao ?? throw new System.ArgumentNullException(nameof(servicoNotificacao));
            this.workflowAprovacaoNivel = workflowAprovacaoNivel ?? throw new System.ArgumentNullException(nameof(workflowAprovacaoNivel));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioAula = repositorioAula ?? throw new ArgumentException(nameof(repositorioAula));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
        }

        public async Task Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId)
        {
            WorkflowAprovacaoNivel nivel = workflow.ObterNivelPorNotificacaoId(notificacaoId);

            nivel.PodeAprovar();

            var niveisParaPersistir = workflow.ModificarStatusPorNivel(aprovar ? WorkflowAprovacaoNivelStatus.Aprovado : WorkflowAprovacaoNivelStatus.Reprovado, nivel.Nivel, observacao);
            AtualizaNiveis(niveisParaPersistir);

            var codigoDaNotificacao = nivel.Notificacoes
                .FirstOrDefault(a => a.Id == notificacaoId).Codigo;

            if (aprovar)
                await AprovarNivel(nivel, workflow, codigoDaNotificacao);
            else await ReprovarNivel(workflow, codigoDaNotificacao, observacao, nivel.Cargo, nivel);
        }

        public void ConfiguracaoInicial(WorkflowAprovacao workflowAprovacao, long idEntidadeParaAprovar)
        {
            if (workflowAprovacao.NotificacaoCategoria == NotificacaoCategoria.Workflow_Aprovacao)
            {
                var niveisIniciais = workflowAprovacao.ObtemNiveis(workflowAprovacao.ObtemPrimeiroNivel()).ToList();
                EnviaNotificacaoParaNiveis(niveisIniciais);
            }
            else
            {
                EnviaNotificacaoParaNiveis(workflowAprovacao.Niveis.ToList());
            }
        }

        public async Task ExcluirWorkflowNotificacoes(long id)
        {
            var workflow = repositorioWorkflowAprovacao.ObterEntidadeCompleta(id);

            if (workflow == null)
                throw new NegocioException("Não foi possível localizar o fluxo de aprovação.");

            if (workflow.Niveis.Any(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                return;

            foreach (WorkflowAprovacaoNivel wfNivel in workflow.Niveis)
            {
                wfNivel.Status = WorkflowAprovacaoNivelStatus.Excluido;
                workflowAprovacaoNivel.Salvar(wfNivel);

                foreach (Notificacao notificacao in wfNivel.Notificacoes)
                {
                    repositorioWorkflowAprovacaoNivelNotificacao.ExcluirPorWorkflowNivelNotificacaoId(wfNivel.Id, notificacao.Id);
                    repositorioNotificacao.Remover(notificacao);
                }
            }

            workflow.Excluido = true;
            await repositorioWorkflowAprovacao.SalvarAsync(workflow);
        }

        private async Task AprovarNivel(WorkflowAprovacaoNivel nivel, WorkflowAprovacao workflow, long codigoDaNotificacao)
        {
            var niveis = workflow.ObtemNiveisParaEnvioPosAprovacao();
            if (niveis != null)
                EnviaNotificacaoParaNiveis(niveis.ToList(), codigoDaNotificacao);
            else
            {
                if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional)
                {
                    AprovarUltimoNivelEventoLiberacaoExcepcional(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.ReposicaoAula)
                {
                    await AprovarUltimoNivelDaReposicaoAula(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Data_Passada)
                {
                    AprovarUltimoNivelDeEventoDataPassada(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.Fechamento_Reabertura)
                {
                    AprovarUltimoNivelDeEventoFechamentoReabertura(codigoDaNotificacao, workflow.Id, nivel.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoNotaFechamento)
                    await AprovarAlteracaoNotaFechamento(codigoDaNotificacao, workflow.Id, workflow.TurmaId);
            }
        }

        private async Task AprovarAlteracaoNotaFechamento(long codigoDaNotificacao, long workFlowId, string turmaCodigo)
        {
            var notasEmAprovacao = ObterNotasEmAprovacao(workFlowId);
            if (notasEmAprovacao != null && notasEmAprovacao.Any())
            {
                AtualizarNotasFechamento(notasEmAprovacao);

                await NotificarAprovacaoNotasFechamento(notasEmAprovacao, codigoDaNotificacao, turmaCodigo);
            }
        }

        private void AtualizarNotasFechamento(IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao)
        {
            var fechamentoTurmaDisciplinaId = notasEmAprovacao.First().FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplinaId;

            unitOfWork.IniciarTransacao();
            try
            {
                // Resolve a pendencia de fechamento
                repositorioPendencia.AtualizarPendencias(fechamentoTurmaDisciplinaId, SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento);

                foreach (var notaEmAprovacao in notasEmAprovacao)
                {
                    var fechamentoNota = notaEmAprovacao.FechamentoNota;

                    fechamentoNota.Nota = notaEmAprovacao.Nota;
                    fechamentoNota.ConceitoId = notaEmAprovacao.ConceitoId;

                    repositorioFechamentoNota.Salvar(fechamentoNota);
                }
                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task AprovarUltimoNivelDaReposicaoAula(long codigoDaNotificacao, long workflowId)
        {
            Aula aula = repositorioAula.ObterPorWorkflowId(workflowId);
            if (aula == null)
                throw new NegocioException("Não foi possível localizar a aula deste fluxo de aprovação.");

            aula.AprovaWorkflow();
            repositorioAula.Salvar(aula);

            await NotificarCriadorDaAulaQueFoiAprovada(aula, codigoDaNotificacao);
        }

        private void AprovarUltimoNivelDeEventoDataPassada(long codigoDaNotificacao, long workflowId)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflowId);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.AprovarWorkflow();
            repositorioEvento.Salvar(evento);

            NotificarCriadorEventoDataPassadaAprovado(evento, codigoDaNotificacao);
            NotificarDiretorUeEventoDataPassadaAprovado(evento, codigoDaNotificacao);
        }

        private void AprovarUltimoNivelDeEventoFechamentoReabertura(long codigoDaNotificacao, long workflowId, long nivelId)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflowId);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.AprovarWorkFlow();


            CriarEventoFechamentoReabertura(fechamentoReabertura);

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            NotificarAdminSgpUeFechamentoReaberturaAprovado(fechamentoReabertura, codigoDaNotificacao, nivelId);
            NotificarDiretorUeFechamentoReaberturaAprovado(fechamentoReabertura, codigoDaNotificacao, nivelId);
        }

        private void CriarEventoFechamentoReabertura(FechamentoReabertura fechamentoReabertura)
        {
            var tipoEvento = repositorioEventoTipo.ObterTipoEventoPorTipo(TipoEvento.FechamentoBimestre);
            if (tipoEvento == null)
                throw new NegocioException($"Não foi possível localizar o tipo de evento {TipoEvento.FechamentoBimestre.GetAttribute<DisplayAttribute>().Name}.");

            var evento = new Evento()
            {
                DataFim = fechamentoReabertura.Fim,
                DataInicio = fechamentoReabertura.Inicio,
                Descricao = fechamentoReabertura.Descricao,
                Nome = $"Reabertura de fechamento de bimestre - {fechamentoReabertura.TipoCalendario.Nome} - {fechamentoReabertura.TipoCalendario.AnoLetivo}.",
                TipoCalendarioId = fechamentoReabertura.TipoCalendario.Id,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                UeId = fechamentoReabertura.Ue.CodigoUe,
                Status = EntidadeStatus.Aprovado,
                TipoEventoId = tipoEvento.Id,
                Migrado = false,
                Letivo = EventoLetivo.Sim,
            };
            repositorioEvento.Salvar(evento);
        }

        private void AprovarUltimoNivelEventoLiberacaoExcepcional(long codigoDaNotificacao, long workflowId)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflowId);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.AprovarWorkflow();
            repositorioEvento.Salvar(evento);

            NotificarCriadorEventoLiberacaoExcepcionalAprovado(evento, codigoDaNotificacao);
        }

        private void AtualizaNiveis(IEnumerable<WorkflowAprovacaoNivel> niveis)
        {
            foreach (var nivel in niveis)
            {
                workflowAprovacaoNivel.Salvar(nivel);

                foreach (var notificacao in nivel.Notificacoes)
                {
                    repositorioNotificacao.Salvar(notificacao);
                }
            }
        }

        private void EnviaNotificacaoParaNiveis(List<WorkflowAprovacaoNivel> niveis, long codigoNotificacao = 0)
        {
            if (codigoNotificacao == 0)
                codigoNotificacao = servicoNotificacao.ObtemNovoCodigo();

            List<Cargo?> cargosNotificados = new List<Cargo?>();

            foreach (var aprovaNivel in niveis)
            {
                if (!cargosNotificados.Contains(aprovaNivel.Cargo))
                    cargosNotificados.Add(EnviaNotificacaoParaNivel(aprovaNivel, codigoNotificacao));
            }
        }

        private Cargo? EnviaNotificacaoParaNivel(WorkflowAprovacaoNivel nivel, long codigoNotificacao)
        {
            Notificacao notificacao;
            List<Usuario> usuarios = nivel.Usuarios.ToList();

            if (nivel.Cargo.HasValue)
            {
                var funcionariosRetorno = servicoNotificacao.ObterFuncionariosPorNivel(nivel.Workflow.UeId, nivel.Cargo);

                foreach (var funcionario in funcionariosRetorno)
                    usuarios.Add(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id));
            }

            foreach (var usuario in usuarios)
            {
                notificacao = new Notificacao()
                {
                    Ano = nivel.Workflow.Ano,
                    Categoria = nivel.Workflow.NotificacaoCategoria,
                    DreId = nivel.Workflow.DreId,
                    UeId = nivel.Workflow.UeId,
                    Mensagem = nivel.Workflow.NotifacaoMensagem,
                    Tipo = nivel.Workflow.NotificacaoTipo,
                    Titulo = nivel.Workflow.NotifacaoTitulo,
                    TurmaId = nivel.Workflow.TurmaId,
                    UsuarioId = usuario.Id,
                    Codigo = codigoNotificacao
                };

                nivel.Adicionar(notificacao);

                repositorioNotificacao.Salvar(notificacao);

                repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao()
                {
                    NotificacaoId = notificacao.Id,
                    WorkflowAprovacaoNivelId = nivel.Id
                });

                if (notificacao.Categoria == NotificacaoCategoria.Workflow_Aprovacao)
                {
                    nivel.Status = WorkflowAprovacaoNivelStatus.AguardandoAprovacao;
                    workflowAprovacaoNivel.Salvar(nivel);
                }
            }

            return nivel.Cargo;
        }

        private async Task NotificarAprovacaoNotasFechamento(IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao, long codigoDaNotificacao, string turmaCodigo, bool aprovada = true, string justificativa = "")
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            var usuarioRf = notasEmAprovacao.First().FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.AlteradoRF;
            var periodoEscolar = notasEmAprovacao.First().FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar;
            var notaConceitoTitulo = notasEmAprovacao.First().ConceitoId.HasValue ? "conceito" : "nota";
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(usuarioRf, "");

            if (usuario != null)
            {
                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = turma.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = DateTime.Today.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = turma.Ue.Dre.CodigoDre,
                    Titulo = $"Alteração em {notaConceitoTitulo} final - Turma {turma.Nome} ({turma.AnoLetivo})",
                    Tipo = NotificacaoTipo.Notas,
                    Codigo = codigoDaNotificacao,
                    Mensagem = MontaMensagemAprovacaoNotaFechamento(turma, usuario, periodoEscolar.Bimestre, notaConceitoTitulo, notasEmAprovacao, aprovada, justificativa)
                });

            }
        }

        private string MontaMensagemAprovacaoNotaFechamento(Turma turma, Usuario usuario, int bimestre, string notaConceitoTitulo, IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao, bool aprovado, string justificativa)
        {
            var aprovadaRecusada = aprovado ? "aprovada" : "recusada";
            var motivo = aprovado ? "" : $"Motivo: {justificativa}.";

            var mensagem = new StringBuilder($@"<p>A alteração de {notaConceitoTitulo}(s) final(is) da turma {turma.Nome} da 
                            {turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} (DRE {turma.Ue.Dre.Nome}) 
                            no bimestre {bimestre} de {turma.AnoLetivo} para o(s) aluno(s) abaxo foi {aprovadaRecusada}. {motivo}</p>");

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 5px;'>Código Aluno</td>");
            mensagem.AppendLine("<td style='padding: 5px;'>Nome do aluno</td>");
            mensagem.AppendLine("</tr>");

            var alunosTurma = servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma).Result;
            foreach (var notaAprovacao in notasEmAprovacao)
            {
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo);

                mensagem.AppendLine("<tr>");
                mensagem.Append($"<td style='padding: 5px;'>{notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo}</td>");
                mensagem.Append($"<td style='padding: 5px;'>{aluno?.NomeAluno}</td>");
                mensagem.AppendLine("</tr>");
            }
            mensagem.AppendLine("</table>");

            return mensagem.ToString();
        }

        private void NotificarAdminSgpUeFechamentoReaberturaAprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, long nivelId)
        {
            var adminsSgpUe = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Ue.CodigoUe).Result;
            if (adminsSgpUe != null && adminsSgpUe.Any())
            {
                foreach (var adminSgpUe in adminsSgpUe)
                {
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                    var notificacao = new Notificacao()
                    {
                        UeId = fechamentoReabertura.Ue.CodigoUe,
                        UsuarioId = usuario.Id,
                        Ano = fechamentoReabertura.CriadoEm.Year,
                        Categoria = NotificacaoCategoria.Aviso,
                        DreId = fechamentoReabertura.Dre.CodigoDre,
                        Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                        Tipo = NotificacaoTipo.Calendario,
                        Codigo = codigoDaNotificacao,
                        Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.TipoEscola.ShortName()} {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi aprovado pela supervisão escolar. <br/>
                                  Tipo de Calendário: {fechamentoReabertura.TipoCalendario.Nome}<br/>
                                  Descrição: { fechamentoReabertura.Descricao} <br/>
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                    };
                    repositorioNotificacao.Salvar(notificacao);

                    repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacao.Id, WorkflowAprovacaoNivelId = nivelId });
                }
            }
        }

        private void NotificarAdminSgpUeFechamentoReaberturaReprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, string motivo, long nivelId)
        {
            var adminsSgpUe = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Ue.CodigoUe).Result;
            if (adminsSgpUe != null && adminsSgpUe.Any())
            {
                foreach (var adminSgpUe in adminsSgpUe)
                {
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                    var notificacao = new Notificacao()
                    {
                        UeId = fechamentoReabertura.Ue.CodigoUe,
                        UsuarioId = usuario.Id,
                        Ano = fechamentoReabertura.CriadoEm.Year,
                        Categoria = NotificacaoCategoria.Aviso,
                        DreId = fechamentoReabertura.Dre.CodigoDre,
                        Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                        Tipo = NotificacaoTipo.Calendario,
                        Codigo = codigoDaNotificacao,
                        Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi reprovado pela supervisão escolar. Motivo: {motivo} <br/>
                                  Descrição: { fechamentoReabertura.Descricao} < br />
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} < br />
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} < br />
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                    };
                    repositorioNotificacao.Salvar(notificacao);
                    repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacao.Id, WorkflowAprovacaoNivelId = nivelId });
                }
            }
        }

        private async Task NotificarAulaReposicaoQueFoiReprovada(Aula aula, long codigoDaNotificacao, string motivo)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(aula.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.CriadoRF);

            repositorioNotificacao.Salvar(new Notificacao()
            {
                UeId = aula.UeId,
                UsuarioId = usuario.Id,
                Ano = aula.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = turma.Ue?.Dre?.CodigoDre,
                Titulo = $"Criação de Aula de Reposição na turma {turma.Nome}",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $"A criação de {aula.Quantidade} aula(s) de reposição de {turma.ModalidadeCodigo.ToString()} na turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Nome}) foi recusada. Motivo: {motivo} "
            });
        }

        private async Task NotificarCriadorDaAulaQueFoiAprovada(Aula aula, long codigoDaNotificacao)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(aula.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.CriadoRF);

            repositorioNotificacao.Salvar(new Notificacao()
            {
                UeId = aula.UeId,
                UsuarioId = usuario.Id,
                Ano = aula.CriadoEm.Year,
                DreId = turma.Ue?.Dre?.CodigoDre,
                Categoria = NotificacaoCategoria.Aviso,
                Titulo = $"Criação de Aula de Reposição na turma {turma.Nome} ",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $" Criação de {aula.Quantidade} aula(s) de reposição de {turma.ModalidadeCodigo.ToString()} na turma {turma.Nome} da {turma.Ue?.Nome} ({turma?.Ue?.Dre?.Nome}) foi aceita."
            });
        }

        private void NotificarCriadorEventoDataPassadaAprovado(Evento evento, long codigoDaNotificacao)
        {
            var escola = repositorioUe.ObterPorCodigo(evento.UeId);

            if (escola == null)
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/:{evento.Id}/";

            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);

            repositorioNotificacao.Salvar(new Notificacao()
            {
                UeId = evento.UeId,
                UsuarioId = usuario.Id,
                Ano = evento.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = evento.DreId,
                Titulo = "Criação de evento com data passada",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola.Nome} foi aceito. Agora este evento está visível para todos os usuários. Para visualizá-lo clique <a href='{linkParaEvento}'>aqui</a>."
            });
        }

        private void NotificarCriadorEventoDataPassadaReprovacao(Evento evento, long codigoDaNotificacao, string motivoRecusa)
        {
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);

            var escola = repositorioUe.ObterPorCodigo(evento.UeId);

            if (escola == null)
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            repositorioNotificacao.Salvar(new Notificacao()
            {
                UeId = evento.UeId,
                UsuarioId = usuario.Id,
                Ano = evento.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = evento.DreId,
                Titulo = "Criação de evento com data passada",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola.Nome} foi recusado. <br/> Motivo: {motivoRecusa}"
            });
        }

        private void NotificarCriadorEventoLiberacaoExcepcionalAprovado(Evento evento, long codigoDaNotificacao)
        {
            var escola = repositorioUe.ObterPorCodigo(evento.UeId);

            if (escola == null)
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/:{evento.Id}/";

            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);

            repositorioNotificacao.Salvar(new Notificacao()
            {
                UeId = evento.UeId,
                UsuarioId = usuario.Id,
                Ano = evento.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = evento.DreId,
                Titulo = "Criação de Eventos Excepcionais",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola.Nome} foi aceito. Agora este evento está visível para todos os usuários. Para visualizá-lo clique <a href='{linkParaEvento}'>aqui</a>."
            });
        }

        private void NotificarDiretorUeEventoDataPassadaAprovado(Evento evento, long codigoDaNotificacao)
        {
            var escola = repositorioUe.ObterPorCodigo(evento.UeId);

            if (escola == null)
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            var funcionariosEscola = servicoNotificacao.ObterFuncionariosPorNivel(escola.CodigoUe, Cargo.Diretor);

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/{evento.Id}/";

            foreach (var funcionario in funcionariosEscola)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id);

                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = evento.UeId,
                    UsuarioId = usuario.Id,
                    Ano = evento.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = evento.DreId,
                    Titulo = "Criação de evento com data passada",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola.Nome} foi aceito. Agora este evento está visível para todos os usuários. Para visualizá-lo clique <a href='{linkParaEvento}'>aqui</a>."
                });
            }
        }

        private void NotificarDiretorUeFechamentoReaberturaAprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, long nivelId)
        {
            var diretoresDaEscola = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);

            if (diretoresDaEscola == null || !diretoresDaEscola.Any())
                throw new NegocioException("Não foi possível localizar o diretor da Ue desta reabertura de fechamento.");
            else
                foreach (var diretorDaEscola in diretoresDaEscola)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretorDaEscola.CodigoRf);
                var notificacao = new Notificacao()
                {
                    UeId = fechamentoReabertura.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = fechamentoReabertura.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = fechamentoReabertura.Dre.CodigoDre,
                    Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.TipoEscola.ShortName()} {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi aprovado pela supervisão escolar. <br/>
                                  Tipo de Calendário: {fechamentoReabertura.TipoCalendario.Nome}<br/>
                                  Descrição: { fechamentoReabertura.Descricao} <br/>
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                    };
                    repositorioNotificacao.Salvar(notificacao);

                    repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacao.Id, WorkflowAprovacaoNivelId = nivelId });
                }
        }

        private void NotificarDiretorUeFechamentoReaberturaReprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, string motivo, long nivelId)
        {
            var diretoresDaEscola = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);

            if (diretoresDaEscola == null || !diretoresDaEscola.Any())
                throw new NegocioException("Não foi possível localizar o diretor da Ue desta reabertura de fechamento.");
            else
            foreach (var diretorDaEscola in diretoresDaEscola)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretorDaEscola.CodigoRf);
                var notificacao = new Notificacao()
                {
                    UeId = fechamentoReabertura.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = fechamentoReabertura.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = fechamentoReabertura.Dre.CodigoDre,
                    Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi reprovado pela supervisão escolar. Motivo: {motivo} <br/>
                                  Descrição: { fechamentoReabertura.Descricao} < br />
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} < br />
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} < br />
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                    };
                    repositorioNotificacao.Salvar(notificacao);
                    repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacao.Id, WorkflowAprovacaoNivelId = nivelId });
                }
        }

        private void NotificarEventoQueFoiReprovado(Evento evento, long codigoDaNotificacao, Usuario usuario, string motivoRecusa, string nomeEscola)
        {
            repositorioNotificacao.Salvar(new Notificacao()
            {
                UeId = evento.UeId,
                UsuarioId = usuario.Id,
                Ano = evento.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = evento.DreId,
                Titulo = "Criação de Eventos Excepcionais",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {nomeEscola} foi recusado. <br/> Motivo: {motivoRecusa}"
            });
        }

        private async Task ReprovarNivel(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou, WorkflowAprovacaoNivel nivel)
        {
            if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional)
            {
                TrataReprovacaoEventoLiberacaoExcepcional(workflow, codigoDaNotificacao, motivo, cargoDoNivelQueRecusou);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.ReposicaoAula)
            {
                await TrataReprovacaoReposicaoAula(workflow, codigoDaNotificacao, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Data_Passada)
            {
                TrataReprovacaoEventoDataPassada(workflow, codigoDaNotificacao, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.Fechamento_Reabertura)
            {
                TrataReprovacaoFechamentoReabertura(workflow, codigoDaNotificacao, motivo, nivel.Id);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoNotaFechamento)
                await TrataReprovacaoAlteracaoNotaFechamento(workflow, codigoDaNotificacao, motivo);
        }

        private async Task TrataReprovacaoAlteracaoNotaFechamento(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            var notasEmAprovacao = ObterNotasEmAprovacao(workflow.Id);

            await NotificarAprovacaoNotasFechamento(notasEmAprovacao, codigoDaNotificacao, workflow.TurmaId, false, motivo);
        }

        private IEnumerable<WfAprovacaoNotaFechamento> ObterNotasEmAprovacao(long workflowId)
            => repositorioFechamentoNota.ObterNotasEmAprovacaoWf(workflowId).Result;

        private void TrataReprovacaoEventoDataPassada(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflow.Id);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.ReprovarWorkflow();
            repositorioEvento.Salvar(evento);

            NotificarCriadorEventoDataPassadaReprovacao(evento, codigoDaNotificacao, motivo);
        }

        private void TrataReprovacaoEventoLiberacaoExcepcional(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflow.Id);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.ReprovarWorkflow();
            repositorioEvento.Salvar(evento);

            var escola = repositorioUe.ObterPorCodigo(evento.UeId);

            if (escola == null)
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            if (cargoDoNivelQueRecusou == Cargo.Supervisor)
            {
                var funcionariosRetorno = servicoNotificacao.ObterFuncionariosPorNivel(evento.UeId, Cargo.Diretor);

                foreach (var funcionario in funcionariosRetorno)
                {
                    var usuarioDiretor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id);

                    NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuarioDiretor, motivo, escola.Nome);
                }
            }
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);
            NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuario, motivo, escola.Nome);
        }

        private void TrataReprovacaoFechamentoReabertura(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, long nivelId)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflow.Id);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.ReprovarWorkFlow();

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            NotificarAdminSgpUeFechamentoReaberturaReprovado(fechamentoReabertura, codigoDaNotificacao, motivo, nivelId);
            NotificarDiretorUeFechamentoReaberturaReprovado(fechamentoReabertura, codigoDaNotificacao, motivo, nivelId);
        }

        private async Task TrataReprovacaoReposicaoAula(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            Aula aula = repositorioAula.ObterPorWorkflowId(workflow.Id);
            if (aula == null)
                throw new NegocioException("Não foi possível localizar a aula deste fluxo de aprovação.");

            aula.ReprovarWorkflow();
            repositorioAula.Salvar(aula);

            await NotificarAulaReposicaoQueFoiReprovada(aula, codigoDaNotificacao, motivo);
        }
    }
}