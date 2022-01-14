using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoWorkflowAprovacao : IServicoWorkflowAprovacao
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel;
        private readonly IMediator mediator;

        public ServicoWorkflowAprovacao(IRepositorioNotificacao repositorioNotificacao,
                                        IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao,
                                        IServicoEol servicoEOL,
                                        IServicoUsuario servicoUsuario,
                                        IServicoNotificacao servicoNotificacao,
                                        IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel,
                                        IRepositorioEvento repositorioEvento,
                                        IConfiguration configuration,
                                        IRepositorioAulaConsulta repositorioAula,
                                        IRepositorioTurmaConsulta repositorioTurma,
                                        IRepositorioUeConsulta repositorioUe,
                                        IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
                                        IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                        IRepositorioFechamentoNota repositorioFechamentoNota,
                                        IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota,
                                        IRepositorioPendencia repositorioPendencia,
                                        IMediator mediator)
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
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId)
        {
            WorkflowAprovacaoNivel nivel = workflow.ObterNivelPorNotificacaoId(notificacaoId);

            var codigoDaNotificacao = nivel.Notificacoes.FirstOrDefault(a => a.Id == notificacaoId)?.Codigo;
            if (codigoDaNotificacao == null)
                throw new NegocioException("Não foi possível localizar a notificação.");

            nivel.PodeAprovar();

            var niveisParaPersistir = workflow.ModificarStatusPorNivel(aprovar ? WorkflowAprovacaoNivelStatus.Aprovado : WorkflowAprovacaoNivelStatus.Reprovado, nivel.Nivel, observacao);
            AtualizaNiveis(niveisParaPersistir);

            if (aprovar)
                await AprovarNivel(nivel, workflow, (long)codigoDaNotificacao);
            else await ReprovarNivel(workflow, (long)codigoDaNotificacao, observacao, nivel.Cargo, nivel);
        }

        public async Task ConfiguracaoInicial(WorkflowAprovacao workflowAprovacao, long idEntidadeParaAprovar)
        {
            if (workflowAprovacao.NotificacaoCategoria == NotificacaoCategoria.Workflow_Aprovacao)
            {
                var niveisIniciais = workflowAprovacao.ObtemNiveis(workflowAprovacao.ObtemPrimeiroNivel()).ToList();
                await EnviaNotificacaoParaNiveis(niveisIniciais);
            }
            else
            {
                await EnviaNotificacaoParaNiveis(workflowAprovacao.Niveis.ToList());
            }
        }

        public async Task ConfiguracaoInicialAsync(WorkflowAprovacao workflowAprovacao, long idEntidadeParaAprovar)
        {
            if (workflowAprovacao.NotificacaoCategoria == NotificacaoCategoria.Workflow_Aprovacao)
            {
                var niveisIniciais = workflowAprovacao.ObtemNiveis(workflowAprovacao.ObtemPrimeiroNivel()).ToList();
                await EnviaNotificacaoParaNiveisAsync(niveisIniciais);
            }
            else
            {
                await EnviaNotificacaoParaNiveisAsync(workflowAprovacao.Niveis.ToList());
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
            if (niveis != null && niveis.Any())
                await EnviaNotificacaoParaNiveis(niveis.ToList(), codigoDaNotificacao);
            else
            {
                if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional)
                {
                    await AprovarUltimoNivelEventoLiberacaoExcepcional(codigoDaNotificacao, workflow.Id);
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
                {
                    await AprovarAlteracaoNotaFechamento(codigoDaNotificacao, workflow.Id, workflow.TurmaId, workflow.CriadoRF, workflow.CriadoPor);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.RegistroItinerancia)
                {
                    await AprovarRegistroDeItinerancia(codigoDaNotificacao, workflow.Id, workflow.CriadoRF, workflow.CriadoPor);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoNotaConselho)
                {
                    await AprovarAlteracaoNotaConselho(workflow.Id, workflow.TurmaId, workflow.CriadoRF, workflow.CriadoPor, codigoDaNotificacao);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoParecerConclusivo)
                {
                    await AprovarAlteracaoParecerConclusivo(workflow.Id, workflow.TurmaId, workflow.CriadoRF, workflow.CriadoPor);
                }
            }
        }

        private async Task ReprovarRegistroDeItinerancia(long workFlowId, string motivo)
        {
            var itineranciaReprovada = await mediator.Send(new ObterWorkflowAprovacaoItineranciaPorIdQuery(workFlowId));
            if (itineranciaReprovada != null)
            {
                await mediator.Send(new AprovarItineranciaCommand(itineranciaReprovada.ItineranciaId, workFlowId, false));

                await mediator.Send(new NotificacaoRegistroItineranciaRecusadoCommand(itineranciaReprovada.ItineranciaId, workFlowId, motivo));
            }
        }

        private async Task AprovarRegistroDeItinerancia(long codigoDaNotificacao, long workFlowId, string criadoRF, string criadoPor)
        {
            var itineranciaEmAprovacao = await mediator.Send(new ObterWorkflowAprovacaoItineranciaPorIdQuery(workFlowId));
            if (itineranciaEmAprovacao != null)
            {
                await mediator.Send(new AprovarItineranciaCommand(itineranciaEmAprovacao.ItineranciaId, workFlowId, true));

                await mediator.Send(new NotificacaoUsuarioCriadorRegistroItineranciaCommand(itineranciaEmAprovacao.ItineranciaId, workFlowId));
            }
        }

        private async Task AprovarAlteracaoParecerConclusivo(long workflowId, string turmaCodigo, string criadoRF, string criadoPor)
        {
            await mediator.Send(new AprovarWorkflowAlteracaoParecerConclusivoCommand(workflowId, turmaCodigo, criadoRF, criadoPor));
        }

        private async Task AprovarAlteracaoNotaConselho(long workflowId, string turmaCodigo, string criadoRF, string criadoPor, long? codigoDaNotificacao)
        {
            await mediator.Send(new AprovarWorkflowAlteracaoNotaConselhoCommand(workflowId, turmaCodigo, criadoRF, criadoPor, codigoDaNotificacao));
        }

        private async Task AprovarAlteracaoNotaFechamento(long codigoDaNotificacao, long workFlowId, string turmaCodigo, string criadoRF, string criadoPor)
        {
            var notasEmAprovacao = await ObterNotasEmAprovacao(workFlowId);
            if (notasEmAprovacao != null && notasEmAprovacao.Any())
            {
                await AtualizarNotasFechamento(notasEmAprovacao, criadoRF, criadoPor, workFlowId);

                await NotificarAprovacaoNotasFechamento(notasEmAprovacao, codigoDaNotificacao, turmaCodigo);
            }
        }

        private async Task AtualizarNotasFechamento(IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao, string criadoRF, string criadoPor, long workFlowId)
        {
            var fechamentoTurmaDisciplinaId = notasEmAprovacao.First().FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplinaId;

            // Resolve a pendencia de fechamento
            repositorioPendencia.AtualizarPendencias(fechamentoTurmaDisciplinaId, SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento);

            foreach (var notaEmAprovacao in notasEmAprovacao)
            {
                var fechamentoNota = notaEmAprovacao.FechamentoNota;

                if (notaEmAprovacao.Nota.HasValue)
                {
                    if (notaEmAprovacao.Nota != fechamentoNota.Nota)
                        await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(fechamentoNota.Nota.Value, notaEmAprovacao.Nota.Value, notaEmAprovacao.FechamentoNotaId, criadoRF, criadoPor, workFlowId));

                    fechamentoNota.Nota = notaEmAprovacao.Nota;
                }
                else
                {
                    if (notaEmAprovacao.ConceitoId != fechamentoNota.ConceitoId)
                        await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(fechamentoNota.ConceitoId.Value, notaEmAprovacao.ConceitoId.Value, notaEmAprovacao.FechamentoNotaId, criadoRF, criadoPor, workFlowId));

                    fechamentoNota.ConceitoId = notaEmAprovacao.ConceitoId;
                }

                repositorioFechamentoNota.Salvar(fechamentoNota);
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

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            NotificarCriadorFechamentoReaberturaAprovado(fechamentoReabertura, codigoDaNotificacao, nivelId);

            NotificarFechamentoReaberturaUEUseCase(fechamentoReabertura);
        }

        private void NotificarFechamentoReaberturaUEUseCase(FechamentoReabertura fechamentoReabertura)
        {
            var usuarioAtual = servicoUsuario.ObterUsuarioLogado().Result;
            mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReaberturaUE, new FiltroNotificacaoFechamentoReaberturaUEDto(MapearFechamentoReaberturaNotificacao(fechamentoReabertura, usuarioAtual)), new Guid(), null));
        }

        private FiltroFechamentoReaberturaNotificacaoDto MapearFechamentoReaberturaNotificacao(FechamentoReabertura fechamentoReabertura, Usuario usuario)
        {
            return new FiltroFechamentoReaberturaNotificacaoDto(fechamentoReabertura.Dre != null ? fechamentoReabertura.Dre.CodigoDre : string.Empty,
                                                                fechamentoReabertura.Ue != null ? fechamentoReabertura.Ue.CodigoUe : string.Empty,
                                                                fechamentoReabertura.Id,
                                                                usuario.CodigoRf,
                                                                fechamentoReabertura.TipoCalendario.Nome,
                                                                fechamentoReabertura.Dre != null ? fechamentoReabertura.Ue.Nome : string.Empty,
                                                                fechamentoReabertura.Dre != null ? fechamentoReabertura.Dre.Abreviacao : string.Empty,
                                                                fechamentoReabertura.Inicio,
                                                                fechamentoReabertura.Fim,
                                                                fechamentoReabertura.ObterBimestresNumeral().ToString(),
                                                                fechamentoReabertura.EhParaUe(),
                                                                fechamentoReabertura.TipoCalendario.AnoLetivo,
                                                                fechamentoReabertura.TipoCalendario.Modalidade.ObterModalidadesTurma().Cast<int>().ToArray());
        }

        private async Task AprovarUltimoNivelEventoLiberacaoExcepcional(long codigoDaNotificacao, long workflowId)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflowId);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.AprovarWorkflow();
            repositorioEvento.Salvar(evento);

            await VerificaPendenciaDiasLetivosInsuficientes(evento);
            NotificarCriadorEventoLiberacaoExcepcionalAprovado(evento, codigoDaNotificacao);
        }

        private async Task VerificaPendenciaDiasLetivosInsuficientes(Evento evento)
        {
            if (evento.EhEventoLetivo())
            {
                var usuario = await servicoUsuario.ObterUsuarioLogado();

                await mediator.Send(new IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand(evento.TipoCalendarioId, evento.DreId, evento.UeId, usuario));
            }
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

        private async Task EnviaNotificacaoParaNiveis(List<WorkflowAprovacaoNivel> niveis, long codigoNotificacao = 0)
        {
            if (codigoNotificacao == 0)
                codigoNotificacao = await servicoNotificacao.ObtemNovoCodigoAsync();

            List<Cargo?> cargosNotificados = new List<Cargo?>();

            foreach (var aprovaNivel in niveis)
            {
                if (!cargosNotificados.Contains(aprovaNivel.Cargo))
                    cargosNotificados.Add(await EnviaNotificacaoParaNivel(aprovaNivel, codigoNotificacao));
            }
        }

        private async Task EnviaNotificacaoParaNiveisAsync(List<WorkflowAprovacaoNivel> niveis, long codigoNotificacao = 0)
        {
            if (codigoNotificacao == 0)
                codigoNotificacao = await servicoNotificacao.ObtemNovoCodigoAsync();

            List<Cargo?> cargosNotificados = new List<Cargo?>();

            foreach (var aprovaNivel in niveis)
            {
                if (!cargosNotificados.Contains(aprovaNivel.Cargo))
                    cargosNotificados.Add(await EnviaNotificacaoParaNivelAsync(aprovaNivel, codigoNotificacao));
            }
        }

        private async Task<Cargo?> EnviaNotificacaoParaNivel(WorkflowAprovacaoNivel nivel, long codigoNotificacao)
        {
            Notificacao notificacao;
            List<Usuario> usuarios = nivel.Usuarios.ToList();

            if (nivel.Cargo.HasValue)
            {
                var funcionariosRetorno = servicoNotificacao.ObterFuncionariosPorNivel(nivel.Workflow.UeId, nivel.Cargo, true, true);

                foreach (var funcionario in funcionariosRetorno)
                {
                    try
                    {
                        usuarios.Add(await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(string.Empty, funcionario.Id, buscaLogin: true));
                    }
                    catch (Exception e)
                    {
                        _ = mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao enviar notificação para nível", LogNivel.Negocio, LogContexto.WorkflowAprovacao, e.Message)).Result;
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

            return nivel.Cargo;
        }

        private async Task<Cargo?> EnviaNotificacaoParaNivelAsync(WorkflowAprovacaoNivel nivel, long codigoNotificacao)
        {
            Notificacao notificacao;
            List<Usuario> usuarios = nivel.Usuarios.ToList();

            if (nivel.Cargo.HasValue)
            {
                var funcionariosRetorno = await servicoNotificacao.ObterFuncionariosPorNivelAsync(nivel.Workflow.UeId, nivel.Cargo, true, true);

                foreach (var funcionario in funcionariosRetorno)
                {
                    try
                    {
                        usuarios.Add(await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(string.Empty, funcionario.Id, buscaLogin: true));
                    }
                    catch (Exception e)
                    {
                        _ = mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao enviar notificação para nível", LogNivel.Negocio, LogContexto.WorkflowAprovacao, e.Message)).Result;
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

                await repositorioNotificacao.SalvarAsync(notificacao);

                await repositorioWorkflowAprovacaoNivelNotificacao.SalvarAsync(new WorkflowAprovacaoNivelNotificacao()
                {
                    NotificacaoId = notificacao.Id,
                    WorkflowAprovacaoNivelId = nivel.Id
                });

                if (notificacao.Categoria == NotificacaoCategoria.Workflow_Aprovacao)
                {
                    nivel.Status = WorkflowAprovacaoNivelStatus.AguardandoAprovacao;
                    await workflowAprovacaoNivel.SalvarAsync(nivel);
                }
            }

            return nivel.Cargo;
        }

        private async Task NotificarAprovacaoNotasFechamento(IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao, long codigoDaNotificacao, string turmaCodigo, bool aprovada = true, string justificativa = "")
        {
            await ExcluirWfNotasFechamento(notasEmAprovacao);

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            var usuarioRf = notasEmAprovacao.First().FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.AlteradoRF;
            var periodoEscolar = notasEmAprovacao.First().FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar;
            var notaConceitoTitulo = notasEmAprovacao.First().ConceitoId.HasValue ? "conceito(s)" : "nota(s)";
            var usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(usuarioRf, ""));
            var componenteCurricularNome = await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(notasEmAprovacao.First().FechamentoNota.DisciplinaId));

            if (usuario != null)
            {
                repositorioNotificacao.Salvar(new Notificacao()
                {
                    UeId = turma.Ue.CodigoUe,
                    UsuarioId = usuario.Id,
                    Ano = DateTime.Today.Year,
                    Categoria = NotificacaoCategoria.Aviso,
                    DreId = turma.Ue.Dre.CodigoDre,
                    Titulo = $"Alteração em {notaConceitoTitulo} final - {componenteCurricularNome} - Turma {turma.Nome} ({turma.AnoLetivo})",
                    Tipo = NotificacaoTipo.Notas,
                    Codigo = codigoDaNotificacao,
                    Mensagem = MontaMensagemAprovacaoNotaFechamento(turma, usuario, periodoEscolar.Bimestre, notaConceitoTitulo, notasEmAprovacao, aprovada, justificativa, componenteCurricularNome)
                });

            }
        }

        private async Task ExcluirWfNotasFechamento(IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao)
        {
            foreach (var notaEmAprovacao in notasEmAprovacao)
                await mediator.Send(new ExcluirWFAprovacaoNotaFechamentoCommand(notaEmAprovacao));
        }

        private string MontaMensagemAprovacaoNotaFechamento(Turma turma, Usuario usuario, int bimestre, string notaConceitoTitulo, IEnumerable<WfAprovacaoNotaFechamento> notasEmAprovacao, bool aprovado, string justificativa, string componenteCurricularNome)
        {
            var aprovadaRecusada = aprovado ? "aprovada" : "recusada";
            var motivo = aprovado ? "" : $"Motivo: {justificativa}.";
            var bimestreFormatado = bimestre == 0 ? "bimestre final" : $"{bimestre}º bimestre";

            var mensagem = new StringBuilder($@"<p>A alteração de {notaConceitoTitulo}(s) final(is) do {bimestreFormatado} do componente curricular {componenteCurricularNome} 
da turma {turma.Nome} da {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) de {turma.AnoLetivo} para o(s) estudante(s) abaixo foi {aprovadaRecusada}. {motivo}</p>");

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Estudante</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Valor anterior</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Novo valor</b></td>");
            mensagem.AppendLine("</tr>");

            var alunosTurma = servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma).Result;

            foreach (var notaAprovacao in notasEmAprovacao)
            {
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo);

                mensagem.AppendLine("<tr>");
                mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo})</td>");

                if (!notaAprovacao.ConceitoId.HasValue)
                {
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.FechamentoNota.Nota)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.Nota.Value)}</td>");
                }
                else
                {
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.FechamentoNota.ConceitoId)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.ConceitoId)}</td>");
                }

                mensagem.AppendLine("</tr>");
            }
            mensagem.AppendLine("</table>");

            return mensagem.ToString();
        }

        private string ObterNota(double? nota)
        {
            if (!nota.HasValue)
                return string.Empty;
            else
                return nota.ToString();
        }

        private string ObterConceito(long? conceitoId)
        {
            if (!conceitoId.HasValue)
                return string.Empty;

            if (conceitoId == (int)ConceitoValores.P)
                return ConceitoValores.P.ToString();
            else if (conceitoId == (int)ConceitoValores.S)
                return ConceitoValores.S.ToString();
            else
                return ConceitoValores.NS.ToString();
        }

        private async Task<string> ObterComponente(long componenteCurricularCodigo)
            => await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(componenteCurricularCodigo));

        private void NotificarCriadorFechamentoReaberturaAprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, long nivelId)
        {
            string criadorRf = fechamentoReabertura.CriadoRF;
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(criadorRf);
            bool isAnoAnterior = fechamentoReabertura.TipoCalendario.AnoLetivo < DateTime.Now.Year;

            var notificacao = new Notificacao()
            {
                UeId = fechamentoReabertura.Ue.CodigoUe,
                UsuarioId = usuario.Id,
                Ano = fechamentoReabertura.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                Titulo =  isAnoAnterior ? "Cadastro de período de reabertura de fechamento - ano anterior" : "Cadastro de período de reabertura de fechamento",
                Tipo = NotificacaoTipo.Calendario,
                Codigo = codigoDaNotificacao,
                Mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.TipoEscola.ObterNomeCurto()} {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi aprovado pela supervisão escolar. <br/>
                                  Tipo de Calendário: {fechamentoReabertura.TipoCalendario.Nome}<br/>
                                  Descrição: { fechamentoReabertura.Descricao} <br/>
                                  Início: { fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                                  Fim: { fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                                  Bimestres: { fechamentoReabertura.ObterBimestresNumeral()}"
            };
            repositorioNotificacao.Salvar(notificacao);

            repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacao.Id, WorkflowAprovacaoNivelId = nivelId });
        }

        private void NotificarCriadorFechamentoReaberturaReprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, string motivo, long nivelId)
        {
            string criadorRF = fechamentoReabertura.CriadoRF;
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(criadorRF);
            bool isAnoAnterior = fechamentoReabertura.TipoCalendario.AnoLetivo < DateTime.Now.Year;
            var notificacao = new Notificacao()
            {
                UeId = fechamentoReabertura.Ue.CodigoUe,
                UsuarioId = usuario.Id,
                Ano = fechamentoReabertura.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                Titulo = isAnoAnterior ? "Cadastro de período de reabertura de fechamento - ano anterior" : "Cadastro de período de reabertura de fechamento",
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

            var funcionariosEscola = servicoNotificacao.ObterFuncionariosPorNivel(escola.CodigoUe, Cargo.Diretor, true, true);

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
            {
                await TrataReprovacaoAlteracaoNotaFechamento(workflow, codigoDaNotificacao, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.RegistroItinerancia)
            {
                await ReprovarRegistroDeItinerancia(workflow.Id, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoNotaConselho)
            {
                await TrataReprovacaoAlteracaoNotaPosConselho(workflow, codigoDaNotificacao, motivo);
            }
            else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoParecerConclusivo)
            {
                await ReprovarParecerConclusivo(workflow.Id, workflow.TurmaId, workflow.CriadoRF, workflow.CriadoPor, motivo);
            }
        }

        private Task ReprovarParecerConclusivo(long workflowId, string turmaCodigo, string criadoRF, string criadoNome, string motivo)
            => mediator.Send(new ReprovarWorkflowAlteracaoParecerConclusivoCommand(workflowId, turmaCodigo, criadoRF, criadoNome, motivo));

        private async Task TrataReprovacaoAlteracaoNotaFechamento(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            var notasEmAprovacao = await ObterNotasEmAprovacao(workflow.Id);

            await NotificarAprovacaoNotasFechamento(notasEmAprovacao, codigoDaNotificacao, workflow.TurmaId, false, motivo);
        }

        private Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacao(long workflowId)
            => mediator.Send(new ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery(workflowId));

        private async Task TrataReprovacaoAlteracaoNotaPosConselho(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            var notasEmAprovacao = await ObterNotaEmAprovacaoPosConselho(workflow.Id);
            await mediator.Send(new RecusarAprovacaoNotaConselhoCommand(notasEmAprovacao,
                                                                        codigoDaNotificacao,
                                                                        workflow.TurmaId,
                                                                        workflow.Id,
                                                                        motivo,
                                                                        notasEmAprovacao.ConselhoClasseNota.Nota,
                                                                        notasEmAprovacao.ConselhoClasseNota.ConceitoId));
        }

        private Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacaoPosConselho(long workflowId)
            => repositorioConselhoClasseNota.ObterNotaEmAprovacaoWf(workflowId);

        private void TrataReprovacaoEventoDataPassada(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflow.Id);
            if (evento == null)
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.ReprovarWorkflow();
            repositorioEvento.Salvar(evento);

            NotificarCriadorEventoDataPassadaReprovacao(evento, codigoDaNotificacao, motivo);
        }

        private async void TrataReprovacaoEventoLiberacaoExcepcional(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou)
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
                var funcionariosRetorno = servicoNotificacao.ObterFuncionariosPorNivel(evento.UeId, Cargo.Diretor, true, true);

                foreach (var funcionario in funcionariosRetorno)
                {
                    var usuarioDiretor = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id);

                    NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuarioDiretor, motivo, escola.Nome);
                }
            }
            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);
            NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuario, motivo, escola.Nome);
        }

        private void TrataReprovacaoFechamentoReabertura(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, long nivelId)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflow.Id);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.ReprovarWorkFlow();

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            NotificarCriadorFechamentoReaberturaReprovado(fechamentoReabertura, codigoDaNotificacao, motivo, nivelId);
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

        public async Task<string> VerificaAulaReposicao(long workflowId, long codigoDaNotificacao)
        {
            if (!await repositorioAula.VerificarAulaPorWorkflowId(workflowId))
            {
                Notificacao notificacao = await mediator.Send(new ObterNotificacaoPorCodigoQuery(codigoDaNotificacao));
                    
                await servicoNotificacao.ExcluirPeloSistemaAsync(new long[notificacao.Id]);
                await ExcluirWorkflowNotificacoes(workflowId);
                return "Não existe aula para esse fluxo de aprovação. A notificação foi excluída.";
            }
            return null;
        }


    }
}