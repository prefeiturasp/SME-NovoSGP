using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel;

        public ServicoWorkflowAprovacao(IRepositorioNotificacao repositorioNotificacao,
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
                                        IRepositorioTurma repositorioTurma,
                                        IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
                                        IUnitOfWork unitOfWork,
                                        IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
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
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
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

        public async Task ExcluirWorkflowNotificacoes(long id)
        {
            var workflow = repositorioWorkflowAprovacao.ObterEntidadeCompleta(id);

            if (workflow == null)
                throw new NegocioException("Não foi possível localizar o fluxo de aprovação.");

            if (workflow.Niveis.Any(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                return;

            unitOfWork.IniciarTransacao();

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

            unitOfWork.PersistirTransacao();
        }

        private void AprovarNivel(WorkflowAprovacaoNivel nivel, long notificacaoId, WorkflowAprovacao workflow, long codigoDaNotificacao)
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
                    AprovarUltimoNivelDaReposicaoAula(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Data_Passada)
                {
                    AprovarUltimoNivelDeEventoDataPassada(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.Fechamento_Reabertura)
                {
                    AprovarUltimoNivelDeEventoFechamentoReabertura(codigoDaNotificacao, workflow.Id);
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

        private void AprovarUltimoNivelDeEventoFechamentoReabertura(long codigoDaNotificacao, long workflowId)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflowId);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.AprovarWorkFlow();
            //TODO: CRIAR EVENTOS;

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            NotificarAdminSgpUeFechamentoReaberturaAprovado(fechamentoReabertura, codigoDaNotificacao);
            NotificarDiretorUeFechamentoReaberturaAprovado(fechamentoReabertura, codigoDaNotificacao);
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

        private void NotificarAdminSgpUeFechamentoReaberturaAprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao)
        {
            var adminsSgpUe = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Ue.CodigoUe).Result;
            if (adminsSgpUe == null || !adminsSgpUe.Any())
                throw new NegocioException("Não foi possível notificar o Administrador SGP da Ue.");

            foreach (var adminSgpUe in adminsSgpUe)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);

                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = fechamentoReabertura.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = fechamentoReabertura.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = fechamentoReabertura.Dre.CodigoDre,
                    Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Nome}) foi aprovado pela supervisão escolar. <br />
                                  Descrição: { fechamentoReabertura.Descricao} < br />
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} < br />
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} < br />
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                });
            }
        }

        private void NotificarAdminSgpUeFechamentoReaberturaReprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, string motivo)
        {
            var adminsSgpUe = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Ue.CodigoUe).Result;
            if (adminsSgpUe == null || !adminsSgpUe.Any())
                throw new NegocioException("Não foi possível notificar o Administrador SGP da Ue.");

            foreach (var adminSgpUe in adminsSgpUe)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);

                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = fechamentoReabertura.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = fechamentoReabertura.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = fechamentoReabertura.Dre.CodigoDre,
                    Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Nome}) foi reprovado pela supervisão escolar. Motivo: {motivo} <br />
                                  Descrição: { fechamentoReabertura.Descricao} < br />
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} < br />
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} < br />
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                });
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

            var diretoresDaEscola = servicoEOL.ObterFuncionariosPorCargoUe(escola.CodigoUe, (long)Cargo.Diretor);

            if (diretoresDaEscola == null && !diretoresDaEscola.Any())
                throw new NegocioException("Não foi possível localizar o diretor da Ue deste evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/{evento.Id}/";

            foreach (var diretor in diretoresDaEscola)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);

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

        private void NotificarDiretorUeFechamentoReaberturaAprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao)
        {
            var diretoresDaEscola = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);

            if (diretoresDaEscola == null && !diretoresDaEscola.Any())
                throw new NegocioException("Não foi possível localizar o diretor da Ue desta reabertura de fechamento.");

            foreach (var diretorDaEscola in diretoresDaEscola)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretorDaEscola.CodigoRf);

                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = fechamentoReabertura.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = fechamentoReabertura.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = fechamentoReabertura.Dre.CodigoDre,
                    Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Nome}) foi aprovado pela supervisão escolar. <br />
                                  Descrição: { fechamentoReabertura.Descricao} < br />
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} < br />
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} < br />
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                });
            }
        }

        private void NotificarDiretorUeFechamentoReaberturaReprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, string motivo)
        {
            var diretoresDaEscola = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);

            if (diretoresDaEscola == null && !diretoresDaEscola.Any())
                throw new NegocioException("Não foi possível localizar o diretor da Ue desta reabertura de fechamento.");

            foreach (var diretorDaEscola in diretoresDaEscola)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretorDaEscola.CodigoRf);

                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = fechamentoReabertura.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = fechamentoReabertura.CriadoEm.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = fechamentoReabertura.Dre.CodigoDre,
                    Titulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                    Tipo = NotificacaoTipo.Calendario,
                    Codigo = codigoDaNotificacao,
                    Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Nome}) foi reprovado pela supervisão escolar. Motivo: {motivo} <br />
                                  Descrição: { fechamentoReabertura.Descricao} < br />
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} < br />
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} < br />
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
                });
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

        private void ReprovarNivel(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou)
        {
            if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional)
            {
                TrataReprovacaoEventoLiberacaoExcepcional(workflow, codigoDaNotificacao, motivo, cargoDoNivelQueRecusou);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.ReposicaoAula)
            {
                TrataReprovacaoReposicaoAula(workflow, codigoDaNotificacao, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Data_Passada)
            {
                TrataReprovacaoEventoDataPassada(workflow, codigoDaNotificacao, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.Fechamento_Reabertura)
            {
                TrataReprovacaoFechamentoReabertura(workflow, codigoDaNotificacao, motivo);
            }
        }

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

        private void TrataReprovacaoFechamentoReabertura(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflow.Id);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.ReprovarWorkFlow();

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            NotificarAdminSgpUeFechamentoReaberturaReprovado(fechamentoReabertura, codigoDaNotificacao, motivo);
            NotificarDiretorUeFechamentoReaberturaReprovado(fechamentoReabertura, codigoDaNotificacao, motivo);
        }

        private void TrataReprovacaoReposicaoAula(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
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