using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoWorkflowAprovacao : IServicoWorkflowAprovacao
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel;

        public ServicoWorkflowAprovacao(IUnitOfWork unitOfWork,
                                        IRepositorioNotificacao repositorioNotificacao,
                                        IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao,
                                        IServicoEOL servicoEOL,
                                        IServicoUsuario servicoUsuario,
                                        IServicoNotificacao servicoNotificacao,
                                        IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel,
                                        IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                        IRepositorioEvento repositorioEvento,
                                        IConfiguration configuration,
                                        IRepositorioAula repositorioAula,
                                        IRepositorioUe repositorioUe,
                                        IRepositorioTurma repositorioTurma)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioWorkflowAprovacaoNivelNotificacao = repositorioWorkflowAprovacaoNivelNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelNotificacao));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoNotificacao = servicoNotificacao ?? throw new System.ArgumentNullException(nameof(servicoNotificacao));
            this.workflowAprovacaoNivel = workflowAprovacaoNivel ?? throw new System.ArgumentNullException(nameof(workflowAprovacaoNivel));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioAula = repositorioAula ?? throw new ArgumentException(nameof(repositorioAula));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public void Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId)
        {
            WorkflowAprovacaoNivel nivel = workflow.ObterNivelPorNotificacaoId(notificacaoId);

            nivel.PodeAprovar();

            var niveisParaPersistir = workflow.ModificarStatusPorNivel(aprovar ? WorkflowAprovacaoNivelStatus.Aprovado : WorkflowAprovacaoNivelStatus.Reprovado, nivel.Nivel, observacao);
            AtualizaNiveis(niveisParaPersistir);

            var codigoDaNotificacao = nivel.Notificacoes
                .FirstOrDefault(a => a.Id == notificacaoId).Codigo;

            if (aprovar)
                AprovarNivel(nivel, notificacaoId, workflow, codigoDaNotificacao);
            else ReprovarNivel(workflow, codigoDaNotificacao, observacao, nivel.Cargo);
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

        private void AprovarNivel(WorkflowAprovacaoNivel nivel, long notificacaoId, WorkflowAprovacao workflow, long codigoDaNotificacao)
        {
            var niveis = workflow.ObtemNiveisParaEnvioPosAprovacao();
            if (niveis != null)
                EnviaNotificacaoParaNiveis(niveis.ToList(), codigoDaNotificacao);
            else
            {
                if (workflow.Tipo == WorkflowAprovacaoTipo.Evento)
                {
                    AprovarUltimoNivelDoEvento(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.ReposicaoAula)
                {
                    AprovarUltimoNivelDaReposicaoAula(codigoDaNotificacao, workflow.Id);
                }
            }
        }

        private void AprovarUltimoNivelDaReposicaoAula(long codigoDaNotificacao, long workflowId)
        {
            Aula aula = repositorioAula.ObterPorWorkflowId(workflowId);
            if (aula == null)
                throw new NegocioException("Não foi possível localizar a aula deste fluxo de aprovação.");

            aula.AprovaWorkflow();
            repositorioAula.Salvar(aula);

            NotificarCriadorDaAulaQueFoiAprovada(aula, codigoDaNotificacao);
        }

        private void AprovarUltimoNivelDoEvento(long codigoDaNotificacao, long workflowId)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflowId);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.AprovarWorkflow();
            repositorioEvento.Salvar(evento);

            NotificarCriadorDoEventoQueFoiAprovado(evento, codigoDaNotificacao);
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

            foreach (var aprovaNivel in niveis)
            {
                EnviaNotificacaoParaNivel(aprovaNivel, codigoNotificacao);
            }
        }

        private void EnviaNotificacaoParaNivel(WorkflowAprovacaoNivel nivel, long codigoNotificacao)
        {
            Notificacao notificacao;
            List<Usuario> usuarios = nivel.Usuarios.ToList();

            if (nivel.Cargo.HasValue)
            {
                if (nivel.Cargo == Cargo.Supervisor)
                {
                    var supervisoresEscola = repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(nivel.Workflow.UeId);
                    if (supervisoresEscola == null || supervisoresEscola.Count() == 0)
                        throw new NegocioException($"Não foram encontrados supervisores atribuídos para a escola de código {nivel.Workflow.UeId} para enviar para o nível {nivel.Nivel}.");

                    foreach (var supervisorEscola in supervisoresEscola)
                    {
                        usuarios.Add(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(supervisorEscola.SupervisorId));
                    }
                }
                else
                {
                    var funcionariosRetornoEol = servicoEOL.ObterFuncionariosPorCargoUe(nivel.Workflow.UeId, (int)nivel.Cargo.Value);
                    if (funcionariosRetornoEol == null || funcionariosRetornoEol.Count() == 0)
                        throw new NegocioException($"Não foram encontrados funcionários de cargo {nivel.Cargo.GetAttribute<DisplayAttribute>().Name} para a escola de código {nivel.Workflow.UeId} para enviar para o nível {nivel.Nivel}.");

                    foreach (var usuario in funcionariosRetornoEol)
                    {
                        usuarios.Add(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuario.CodigoRf));
                    }
                }
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
        }

        private void NotificarAulaReposicaoQueFoiReprovada(Aula aula, long codigoDaNotificacao, string motivo)
        {
            var turma = repositorioTurma.ObterTurmaComUeEDrePorId(aula.TurmaId);
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

        private void NotificarCriadorDaAulaQueFoiAprovada(Aula aula, long codigoDaNotificacao)
        {
            var turma = repositorioTurma.ObterTurmaComUeEDrePorId(aula.TurmaId);
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

        private void NotificarCriadorDoEventoQueFoiAprovado(Evento evento, long codigoDaNotificacao)
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
                Mensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {nomeEscola} foi recusado. < br/> Motivo: {motivoRecusa}"
            });
        }

        private void ReprovarNivel(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou)
        {
            if (workflow.Tipo == WorkflowAprovacaoTipo.Evento)
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
                    var funcionariosRetornoEol = servicoEOL.ObterFuncionariosPorCargoUe(evento.UeId, (int)Cargo.Diretor);
                    if (funcionariosRetornoEol == null || !funcionariosRetornoEol.Any())
                    {
                        throw new NegocioException($"Não foram encontrados funcionários de cargo {Cargo.Diretor.GetAttribute<DisplayAttribute>().Name} para a escola de código {evento.UeId} para enviar a reprovação do evento.");
                    }
                    foreach (var usuarioEol in funcionariosRetornoEol)
                    {
                        var usuarioDiretor = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioEol.CodigoRf);

                        NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuarioDiretor, motivo, escola.Nome);
                    }
                }
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);
                NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuario, motivo, escola.Nome);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.ReposicaoAula)
            {
                Aula aula = repositorioAula.ObterPorWorkflowId(workflow.Id);
                if (aula == null)
                    throw new NegocioException("Não foi possível localizar a aula deste fluxo de aprovação.");

                aula.ReprovarWorkflow();
                repositorioAula.Salvar(aula);

                NotificarAulaReposicaoQueFoiReprovada(aula, codigoDaNotificacao, motivo);
            }
        }
    }
}