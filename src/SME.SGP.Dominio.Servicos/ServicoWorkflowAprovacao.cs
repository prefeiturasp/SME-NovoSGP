using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Constantes;
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
        private readonly IRepositorioWorkflowAprovacaoNivelUsuario repositorioWorkflowAprovacaoNivelUsuario;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel;
        private readonly IMediator mediator;

        public ServicoWorkflowAprovacao(IRepositorioNotificacao repositorioNotificacao,
                                        IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao,
                                        IServicoUsuario servicoUsuario,
                                        IServicoNotificacao servicoNotificacao,
                                        IRepositorioWorkflowAprovacaoNivel workflowAprovacaoNivel,
                                        IRepositorioWorkflowAprovacaoNivelUsuario repositorioWorkflowAprovacaoNivelUsuario,
                                        IRepositorioEvento repositorioEvento,
                                        IConfiguration configuration,
                                        IRepositorioAulaConsulta repositorioAula,
                                        IRepositorioTurmaConsulta repositorioTurma,
                                        IRepositorioUeConsulta repositorioUe,
                                        IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
                                        IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                        IRepositorioFechamentoNota repositorioFechamentoNota,
                                        IRepositorioPendencia repositorioPendencia,
                                        IMediator mediator)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioWorkflowAprovacaoNivelNotificacao = repositorioWorkflowAprovacaoNivelNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelNotificacao));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoNotificacao = servicoNotificacao ?? throw new System.ArgumentNullException(nameof(servicoNotificacao));
            this.workflowAprovacaoNivel = workflowAprovacaoNivel ?? throw new System.ArgumentNullException(nameof(workflowAprovacaoNivel));
            this.repositorioWorkflowAprovacaoNivelUsuario = repositorioWorkflowAprovacaoNivelUsuario ?? throw new System.ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelUsuario));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioAula = repositorioAula ?? throw new ArgumentException(nameof(repositorioAula));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Aprovar(WorkflowAprovacao workflow, bool aprovar, string observacao, long notificacaoId)
        {
            WorkflowAprovacaoNivel nivel = workflow.ObterNivelPorNotificacaoId(notificacaoId);

            var codigoDaNotificacao = nivel.Notificacoes.FirstOrDefault(a => a.Id == notificacaoId)?.Codigo;
            if (codigoDaNotificacao.EhNulo())
                throw new NegocioException("Não foi possível localizar a notificação.");

            nivel.PodeAprovar();

            var niveisParaPersistir = workflow.ModificarStatusPorNivel(aprovar ? WorkflowAprovacaoNivelStatus.Aprovado : WorkflowAprovacaoNivelStatus.Reprovado, nivel.Nivel, observacao);
            await AtualizaNiveis(niveisParaPersistir);

            if (aprovar)
                await AprovarNivel(nivel, workflow, (long)codigoDaNotificacao!);
            else await ReprovarNivel(workflow, (long)codigoDaNotificacao!, observacao, nivel.Cargo, nivel);

            foreach (var notificacao in nivel.Notificacoes)
                await mediator.Send(new NotificarLeituraNotificacaoCommand(notificacao, notificacao.Usuario.CodigoRf));
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
            var workflow = await repositorioWorkflowAprovacao.ObterEntidadeCompleta(id);

            if (workflow.EhNulo())
                throw new NegocioException("Não foi possível localizar o fluxo de aprovação.");

            if (workflow.Niveis.Any(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                return;

            foreach (WorkflowAprovacaoNivel wfNivel in workflow.Niveis)
            {
                wfNivel.Status = WorkflowAprovacaoNivelStatus.Excluido;
                workflowAprovacaoNivel.Salvar(wfNivel);

                foreach (Notificacao notificacao in wfNivel.Notificacoes)
                {
                    await repositorioWorkflowAprovacaoNivelNotificacao.ExcluirPorWorkflowNivelNotificacaoId(wfNivel.Id, notificacao.Id);
                    await mediator.Send(new ExcluirNotificacaoCommand(notificacao));
                }
            }

            workflow.Excluido = true;
            await repositorioWorkflowAprovacao.SalvarAsync(workflow);
        }

        private async Task AprovarNivel(WorkflowAprovacaoNivel nivel, WorkflowAprovacao workflow, long codigoDaNotificacao)
        {
            var niveis = workflow.ObtemNiveisParaEnvioPosAprovacao();
            if (niveis.NaoEhNulo() && niveis.Any())
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
                    await AprovarUltimoNivelDeEventoDataPassada(codigoDaNotificacao, workflow.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.Fechamento_Reabertura)
                {
                    await AprovarUltimoNivelDeEventoFechamentoReabertura(codigoDaNotificacao, workflow.Id, nivel.Id);
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.AlteracaoNotaFechamento)
                {
                    await AprovarAlteracaoNotaFechamento(codigoDaNotificacao, workflow.Id, workflow.TurmaId, ObterUsuarioWf(workflow.CriadoRF, workflow.AlteradoRF), ObterUsuarioWf(workflow.CriadoPor, workflow.AlteradoPor));
                }
                else if (workflow.Tipo == WorkflowAprovacaoTipo.RegistroItinerancia)
                {
                    await AprovarRegistroDeItinerancia(workflow.Id);
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

        private static string ObterUsuarioWf(string criado, string alterado)
        {
            return string.IsNullOrEmpty(alterado) ? criado : alterado;
        }

        private async Task ReprovarRegistroDeItinerancia(long workFlowId, string motivo)
        {
            var itineranciaReprovada = await mediator.Send(new ObterWorkflowAprovacaoItineranciaPorIdQuery(workFlowId));
            if (itineranciaReprovada.NaoEhNulo())
            {
                await mediator.Send(new AprovarItineranciaCommand(itineranciaReprovada.ItineranciaId, workFlowId, false));

                await mediator.Send(new NotificacaoRegistroItineranciaRecusadoCommand(itineranciaReprovada.ItineranciaId, workFlowId, motivo));
            }
        }

        private async Task AprovarRegistroDeItinerancia(long workFlowId)
        {
            var itineranciaEmAprovacao = await mediator.Send(new ObterWorkflowAprovacaoItineranciaPorIdQuery(workFlowId));
            if (itineranciaEmAprovacao.NaoEhNulo())
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
            if (notasEmAprovacao.NaoEhNulo() && notasEmAprovacao.Any())
            {
                var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
                
                var notaTipoValor = await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(turma));
                
                await AtualizarNotasFechamento(notasEmAprovacao, criadoRF, criadoPor, workFlowId, notaTipoValor.TipoNota);

                await NotificarAprovacaoNotasFechamento(notasEmAprovacao, codigoDaNotificacao, turma);
            }
        }

        private async Task AtualizarNotasFechamento(IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasEmAprovacao, string criadoRF, string criadoPor, long workFlowId,TipoNota tipoNota)
        {
            var fechamentoAluno = notasEmAprovacao.First().FechamentoNota.FechamentoAluno;
            var fechamentoTurmaDisciplinaId = fechamentoAluno.FechamentoTurmaDisciplinaId;
            
            // Resolve a pendencia de fechamento
            await repositorioPendencia.AtualizarPendencias(fechamentoTurmaDisciplinaId, SituacaoPendencia.Resolvida, TipoPendencia.AlteracaoNotaFechamento);

            foreach (var notaEmAprovacao in notasEmAprovacao)
            {
                var fechamentoNota = notaEmAprovacao.FechamentoNota;

                if (tipoNota == TipoNota.Nota)
                {
                    if (notaEmAprovacao.WfAprovacao.Nota != fechamentoNota.Nota)
                        await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(fechamentoNota.Nota, notaEmAprovacao.WfAprovacao.Nota, notaEmAprovacao.WfAprovacao.FechamentoNotaId, criadoRF, criadoPor, workFlowId));

                    fechamentoNota.Nota = notaEmAprovacao.WfAprovacao.Nota;
                }
                else
                {
                    if (notaEmAprovacao.WfAprovacao.ConceitoId != fechamentoNota.ConceitoId)
                        await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(fechamentoNota.ConceitoId, notaEmAprovacao.WfAprovacao.ConceitoId, notaEmAprovacao.WfAprovacao.FechamentoNotaId, criadoRF, criadoPor, workFlowId));

                    fechamentoNota.ConceitoId = notaEmAprovacao.WfAprovacao.ConceitoId;
                }
                repositorioFechamentoNota.Salvar(fechamentoNota);

                await AtualizarCacheFechamentoNota(notaEmAprovacao, fechamentoNota);
            }

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync,
                                               new ConsolidacaoTurmaDto(fechamentoAluno.FechamentoTurmaDisciplina.FechamentoTurma.TurmaId, 0),
                                               Guid.NewGuid(),
                                               null));
        }

        private async Task AtualizarNotasFechamento(IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasEmAprovacao)
        {
            var agrupamento = notasEmAprovacao.GroupBy(notaEmAprovacao => new
            {
                notaEmAprovacao.TurmaId,
                notaEmAprovacao.FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.DisciplinaId,
                notaEmAprovacao.FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId
            });
            foreach(var notaEmAprovacao in agrupamento)
                await RemoverCacheFechamentoNota(notaEmAprovacao.Key.TurmaId, notaEmAprovacao.Key.DisciplinaId, notaEmAprovacao.Key.PeriodoEscolarId);
        }

        private async Task AtualizarCacheFechamentoNota(
                                WfAprovacaoNotaFechamentoTurmaDto notaEmAprovacao, 
                                FechamentoNota fechamentoNota)
        {
            await mediator.Send(new AtualizarCacheFechamentoNotaCommand(
                                        fechamentoNota,
                                        notaEmAprovacao.CodigoAluno,
                                        await mediator.Send(new ObterTurmaCodigoPorIdQuery(notaEmAprovacao.TurmaId)),
                                        notaEmAprovacao.FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.DisciplinaId));

            var chaveCacheNotaBimestre = string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_PERIODO_COMPONENTE,
                                            notaEmAprovacao.TurmaId,
                                            notaEmAprovacao.FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId,
                                            notaEmAprovacao.FechamentoNota.FechamentoAluno.FechamentoTurmaDisciplina.DisciplinaId);
            await mediator.Send(new RemoverChaveCacheCommand(chaveCacheNotaBimestre));
        }

        private async Task RemoverCacheFechamentoNota(long turmaId, long disciplinaId, long? periodoEscolarId)
        {
            var codigoTurma = await mediator.Send(new ObterTurmaCodigoPorIdQuery(turmaId));
            var chaveCacheNotaFinal = ObterChaveFechamentoNotaFinalComponenteTurma(disciplinaId.ToString(),
                                                                                   codigoTurma);
            await mediator.Send(new RemoverChaveCacheCommand(chaveCacheNotaFinal));

            var chaveCacheNotaBimestre = string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_PERIODO_COMPONENTE,
                                            turmaId,
                                            periodoEscolarId,
                                            disciplinaId);
            await mediator.Send(new RemoverChaveCacheCommand(chaveCacheNotaBimestre));
        }

        private string ObterChaveFechamentoNotaFinalComponenteTurma(string codigoDisciplina, string codigoTurma)
            => string.Format(NomeChaveCache.FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA, codigoDisciplina, codigoTurma);

        private async Task AprovarUltimoNivelDaReposicaoAula(long codigoDaNotificacao, long workflowId)
        {

            Aula aula = await repositorioAula.ObterPorWorkflowId(workflowId);
            if (aula.EhNulo())
                throw new NegocioException("Não foi possível localizar a aula deste fluxo de aprovação.");

            aula.AprovaWorkflow();
            repositorioAula.Salvar(aula);

            await NotificarCriadorDaAulaQueFoiAprovada(aula, codigoDaNotificacao);
        }

        private async Task AprovarUltimoNivelDeEventoDataPassada(long codigoDaNotificacao, long workflowId)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflowId);
            if (evento.EhNulo())
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.AprovarWorkflow();
            await repositorioEvento.SalvarAsync(evento);

            await NotificarCriadorEventoDataPassadaAprovado(evento, codigoDaNotificacao);
            await NotificarDiretorUeEventoDataPassadaAprovado(evento, codigoDaNotificacao);
        }

        private async Task AprovarUltimoNivelDeEventoFechamentoReabertura(long codigoDaNotificacao, long workflowId, long nivelId)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflowId);
            if (fechamentoReabertura.EhNulo())
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.AprovarWorkFlow();

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            await NotificarCriadorFechamentoReaberturaAprovado(fechamentoReabertura, codigoDaNotificacao, nivelId);

            NotificarFechamentoReaberturaUEUseCase(fechamentoReabertura);
        }

        private void NotificarFechamentoReaberturaUEUseCase(FechamentoReabertura fechamentoReabertura)
        {
            var usuarioAtual = servicoUsuario.ObterUsuarioLogado().Result;
            mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReaberturaUE, new FiltroNotificacaoFechamentoReaberturaUEDto(MapearFechamentoReaberturaNotificacao(fechamentoReabertura, usuarioAtual)), Guid.NewGuid(), null));
        }

        private FiltroFechamentoReaberturaNotificacaoDto MapearFechamentoReaberturaNotificacao(FechamentoReabertura fechamentoReabertura, Usuario usuario)
        {
            return new FiltroFechamentoReaberturaNotificacaoDto(fechamentoReabertura.Dre.NaoEhNulo() ? fechamentoReabertura.Dre.CodigoDre : string.Empty,
                                                                fechamentoReabertura.Ue.NaoEhNulo() ? fechamentoReabertura.Ue.CodigoUe : string.Empty,
                                                                fechamentoReabertura.Id,
                                                                usuario.CodigoRf,
                                                                fechamentoReabertura.TipoCalendario.Nome,
                                                                fechamentoReabertura.Dre.NaoEhNulo() ? fechamentoReabertura.Ue.Nome : string.Empty,
                                                                fechamentoReabertura.Dre.NaoEhNulo() ? fechamentoReabertura.Dre.Abreviacao : string.Empty,
                                                                fechamentoReabertura.Inicio,
                                                                fechamentoReabertura.Fim,
                                                                fechamentoReabertura.ObterBimestresNumeral().ToString(),
                                                                fechamentoReabertura.EhParaUe(),
                                                                fechamentoReabertura.TipoCalendario.AnoLetivo,
                                                                fechamentoReabertura.TipoCalendario.Modalidade.ObterModalidades().Cast<int>().ToArray());
        }

        private async Task AprovarUltimoNivelEventoLiberacaoExcepcional(long codigoDaNotificacao, long workflowId)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflowId);
            if (evento.EhNulo())
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.AprovarWorkflow();
            repositorioEvento.Salvar(evento);

            await VerificaPendenciaDiasLetivosInsuficientes(evento);
            await NotificarCriadorEventoLiberacaoExcepcionalAprovado(evento, codigoDaNotificacao);
        }

        private async Task VerificaPendenciaDiasLetivosInsuficientes(Evento evento)
        {
            if (evento.EhEventoLetivo())
            {
                var usuario = await servicoUsuario.ObterUsuarioLogado();

                await mediator.Send(new IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand(evento.TipoCalendarioId, evento.DreId, evento.UeId, usuario));
            }
        }

        private async Task AtualizaNiveis(IEnumerable<WorkflowAprovacaoNivel> niveis)
        {
            foreach (var nivel in niveis)
            {
                await workflowAprovacaoNivel.SalvarAsync(nivel);

                foreach (var notificacao in nivel.Notificacoes)
                {
                    await repositorioNotificacao.SalvarAsync(notificacao);
                    await mediator.Send(new NotificarCriacaoNotificacaoCommand(notificacao));
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

                await repositorioNotificacao.SalvarAsync(notificacao);
                await mediator.Send(new NotificarCriacaoNotificacaoCommand(notificacao, usuario.CodigoRf));

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

                repositorioWorkflowAprovacaoNivelUsuario.Salvar(new WorkflowAprovacaoNivelUsuario()
                {
                    UsuarioId = usuario.Id,
                    WorkflowAprovacaoNivelId = nivel.Id
                });
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
                await mediator.Send(new NotificarCriacaoNotificacaoCommand(notificacao, usuario.CodigoRf));

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

        private async Task<List<Usuario>> ObterUsuariosParaEnviarNotificacoes(WorkflowAprovacaoNivel nivel, Cargo? cargo, string codigoUe)
        {
            List<Usuario> usuarios = nivel.Usuarios.ToList();
            var escola = repositorioUe.ObterPorCodigo(codigoUe);

            if (nivel.Cargo.HasValue)
            {
                var funcionariosRetorno = await ObterFuncionariosAsync(escola.TipoEscola, cargo, nivel.Workflow.UeId);

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
            return usuarios;
        }

        private async Task<IEnumerable<(Cargo? cargo, string Id)>> ObterFuncionariosAsync(
            TipoEscola tipoEscola, Cargo? cargo, string codigoUe)
        {
            if (tipoEscola == TipoEscola.CIEJA && cargo.HasValue && GestaoCIEJA.TryGetValue((tipoEscola, cargo.Value), out var fa))
                return (await servicoNotificacao.ObterFuncionariosPorNivelFuncaoAtividadeAsync(codigoUe, fa, true, true))
                    .Select(f => (cargo, Id: f.Id));

            if ((tipoEscola == TipoEscola.CRPCONV || tipoEscola == TipoEscola.CEIINDIR)
                && cargo.HasValue && GestaoUnidadesConveniadas.TryGetValue((tipoEscola, cargo.Value), out var fe))
                return (await mediator.Send(new ObterFuncionariosPorUeEFuncaoExternaQuery(codigoUe, (int)fe)))
                    .Select(f => (cargo, Id: f.CodigoRF));

            return await servicoNotificacao.ObterFuncionariosPorNivelAsync(codigoUe, cargo, true, true);
        }

        private static readonly Dictionary<(TipoEscola, Cargo?), FuncaoAtividade> GestaoCIEJA =
            new[]
            {
                (Cargo.CP, FuncaoAtividade.COORDERNADOR_PEDAGOGICO_CIEJA),
                (Cargo.AD, FuncaoAtividade.ASSISTENTE_COORDERNADOR_GERAL_CIEJA),
                (Cargo.Diretor, FuncaoAtividade.COORDERNADOR_GERAL_CIEJA)
            }
            .ToDictionary(x => (TipoEscola.CIEJA, (Cargo?)x.Item1), x => x.Item2);

        private static readonly Dictionary<(TipoEscola, Cargo?), FuncaoExterna> GestaoUnidadesConveniadas =
            new[] { TipoEscola.CRPCONV, TipoEscola.CEIINDIR }
                .SelectMany(t => new[]
            {
                    (t, Cargo.CP, FuncaoExterna.CP),
                    (t, Cargo.AD, FuncaoExterna.AD),
                    (t, Cargo.Diretor, FuncaoExterna.Diretor)
             })
             .ToDictionary(x => (x.t, (Cargo?)x.Item2), x => x.Item3);

        private async Task NotificarAprovacaoNotasFechamento(IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasEmAprovacao, long codigoDaNotificacao, Turma turma, bool aprovada = true, string justificativa = "")
        {
            await ExcluirWfNotasFechamento(notasEmAprovacao);

            int? bimestre = notasEmAprovacao.First().Bimestre;
            var notaConceitoTitulo = notasEmAprovacao.First().WfAprovacao.ConceitoId.HasValue ? "conceito(s)" : "nota(s)";
            
            var usuariosRfs = notasEmAprovacao.Select(n => n.UsuarioSolicitanteRf);

            foreach (var usuarioRf in usuariosRfs.Distinct())
            {
                var dadosUsuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(usuarioRf, ""));

                if (dadosUsuario.NaoEhNulo())
                {
                    await mediator.Send(new NotificarUsuarioCommand(
                        $"Alteração em {notaConceitoTitulo} final - {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) - {turma.NomeComModalidade()} (ano anterior)",
                        await MontaMensagemAprovacaoNotaFechamento(turma, bimestre, notaConceitoTitulo, notasEmAprovacao, aprovada, justificativa),
                        usuarioRf,
                        NotificacaoCategoria.Aviso,
                        NotificacaoTipo.Notas,
                        turma.Ue.Dre.CodigoDre,
                        turma.Ue.CodigoUe,
                        turma.CodigoTurma,
                        DateTime.Today.Year,
                        codigoDaNotificacao,
                        nomeUsuario: dadosUsuario.Nome,
                        usuarioId: dadosUsuario.Id));
                }
            }

        }

        private async Task ExcluirWfNotasFechamento(IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasEmAprovacao)
        {
            foreach (var notaEmAprovacao in notasEmAprovacao)
                await mediator.Send(new ExcluirWFAprovacaoNotaFechamentoCommand(notaEmAprovacao.WfAprovacao));
        }

        private async Task<string> MontaMensagemAprovacaoNotaFechamento(Turma turma, int? bimestre, string notaConceitoTitulo, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto> notasEmAprovacao, bool aprovado, string justificativa)
        {
            var aprovadaRecusada = aprovado ? "aprovada" : "recusada";
            var motivo = aprovado ? "" : $"Motivo: {justificativa}.";
            var bimestreFormatado = !bimestre.HasValue ? "bimestre final" : $"{bimestre}º bimestre";

            var mensagem = new StringBuilder($@"<p>A alteração de {notaConceitoTitulo} do {bimestreFormatado} de {turma.AnoLetivo} da turma {turma.ModalidadeCodigo.ObterNomeCurto()}-{turma.Nome} da {turma.Ue.TipoEscola.ObterNomeCurto()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) abaixo foi {aprovadaRecusada}. {motivo}</p>");

            mensagem.AppendLine("<table style='margin-left: auto; margin-right: auto;' border='2' cellpadding='5'>");
            mensagem.AppendLine("<tr>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Componente curricular</b></td>");
            mensagem.AppendLine("<td style='padding: 20px; text-align:left;'><b>Estudante</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Valor anterior</b></td>");
            mensagem.AppendLine("<td style='padding: 5px; text-align:left;'><b>Novo valor</b></td>");
            mensagem.AppendLine("<td style='padding: 10px; text-align:left;'><b>Usuário que alterou</b></td>");
            mensagem.AppendLine("<td style='padding: 10px; text-align:left;'><b>Data da alteração</b></td>");
            mensagem.AppendLine("</tr>");

            var codigoAlunos = notasEmAprovacao.Select(x => long.Parse(x.CodigoAluno)).ToArray();
            var alunosTurma = (await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma, true))).OrderBy(c => c.NomeAluno);

            foreach (var notaAprovacao in notasEmAprovacao)
            {
                var aluno = alunosTurma.FirstOrDefault(c => c.CodigoAluno == (notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo));

                string nomeUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoPor.EhNulo() ? notaAprovacao.WfAprovacao.CriadoPor : notaAprovacao.WfAprovacao.AlteradoPor;
                string rfUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoRF.EhNulo() ? notaAprovacao.WfAprovacao.CriadoRF : notaAprovacao.WfAprovacao.AlteradoRF;
                DateTime? dataUsuarioAlterou = notaAprovacao.WfAprovacao.AlteradoEm.EhNulo() ? notaAprovacao.WfAprovacao.CriadoEm : notaAprovacao.WfAprovacao.AlteradoEm;
                var horaNotificacao = notaAprovacao.WfAprovacao.CriadoEm.ToString("HH:mm:ss");
                var dataNotificacao = notaAprovacao.WfAprovacao.CriadoEm.ToString("dd/MM/yyyy");
                if (notaAprovacao.WfAprovacao.AlteradoEm.HasValue)
                {
                    horaNotificacao = notaAprovacao.WfAprovacao.AlteradoEm.Value.ToString("HH:mm:ss");
                    dataNotificacao = notaAprovacao.WfAprovacao.AlteradoEm.Value.ToString("dd/MM/yyyy");
                }

                mensagem.AppendLine("<tr>");


                if (!notaAprovacao.WfAprovacao.ConceitoId.HasValue)
                {
                    mensagem.Append($"<td style='padding: 20px; text-align:left;'>{notaAprovacao.ComponenteCurricularDescricao}</td>");
                    mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo})</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.WfAprovacao.NotaAnterior)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterNota(notaAprovacao.WfAprovacao.Nota)}</td>");
                    mensagem.Append($"<td style='padding: 10px; text-align:right;'> {nomeUsuarioAlterou} ({rfUsuarioAlterou}) </td>");
                    mensagem.Append($"<td style='padding: 10px; text-align:right;'>{dataNotificacao} ({horaNotificacao}) </td>");
                }
                else
                {
                    mensagem.Append($"<td style='padding: 20px; text-align:left;'>{notaAprovacao.ComponenteCurricularDescricao}</td>");
                    mensagem.Append($"<td style='padding: 20px; text-align:left;'>{aluno?.NumeroAlunoChamada} - {aluno?.NomeAluno} ({notaAprovacao.FechamentoNota.FechamentoAluno.AlunoCodigo})</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.WfAprovacao.ConceitoIdAnterior)}</td>");
                    mensagem.Append($"<td style='padding: 5px; text-align:right;'>{ObterConceito(notaAprovacao.WfAprovacao.ConceitoId)}</td>");
                    mensagem.Append($"<td style='padding: 10px; text-align:right;'> {nomeUsuarioAlterou} ({rfUsuarioAlterou}) </td>");
                    mensagem.Append($"<td style='padding: 10px; text-align:right;'>{dataNotificacao}({horaNotificacao})  </td>");
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

        private async Task NotificarCriadorFechamentoReaberturaAprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, long nivelId)
        {
            string criadorRf = fechamentoReabertura.CriadoRF;

            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(criadorRf));
            bool isAnoAnterior = fechamentoReabertura.TipoCalendario.AnoLetivo < DateTime.Now.Year;

            var mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.TipoEscola.ObterNomeCurto()} {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi aprovado pela supervisão escolar. <br/>
                            Tipo de Calendário: {fechamentoReabertura.TipoCalendario.Nome}<br/>
                            Descrição: {fechamentoReabertura.Descricao} <br/>
                            Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                            Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                            Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}";

            var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(
                isAnoAnterior ? "Cadastro de período de reabertura de fechamento - ano anterior" : "Cadastro de período de reabertura de fechamento",
                mensagem,
                criadorRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                fechamentoReabertura.Dre.CodigoDre,
                fechamentoReabertura.Ue.CodigoUe,
                ano: fechamentoReabertura.CriadoEm.Year,
                codigo: codigoDaNotificacao,
                usuarioId: usuario.Id));

            repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacaoId, WorkflowAprovacaoNivelId = nivelId });

        }

        private async Task NotificarCriadorFechamentoReaberturaReprovado(FechamentoReabertura fechamentoReabertura, long codigoDaNotificacao, string motivo, long nivelId)
        {
            string criadorRF = fechamentoReabertura.CriadoRF;
            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(criadorRF));
            bool isAnoAnterior = fechamentoReabertura.TipoCalendario.AnoLetivo < DateTime.Now.Year;

            var mensagem = $@"O período de reabertura do fechamento de bimestre abaixo da {fechamentoReabertura.Ue.Nome} ({fechamentoReabertura.Dre.Abreviacao}) foi reprovado pela supervisão escolar. Motivo: {motivo} <br/>
                            Descrição: {fechamentoReabertura.Descricao} <br/>
                            Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                            Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                            Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}";

            var notificacaoId = await mediator.Send(new NotificarUsuarioCommand(
                isAnoAnterior ? "Cadastro de período de reabertura de fechamento - ano anterior" : "Cadastro de período de reabertura de fechamento",
                mensagem,
                criadorRF,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                fechamentoReabertura.Dre.CodigoDre,
                fechamentoReabertura.Ue.CodigoUe,
                ano: fechamentoReabertura.CriadoEm.Year,
                codigo: codigoDaNotificacao,
                usuarioId: usuario.Id));

            repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao() { NotificacaoId = notificacaoId, WorkflowAprovacaoNivelId = nivelId });
        }

        private async Task NotificarAulaReposicaoQueFoiReprovada(Aula aula, long codigoDaNotificacao, string motivo)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(aula.TurmaId);
            if (turma.EhNulo())
                throw new NegocioException("Turma não localizada.");

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.CriadoRF);

            await mediator.Send(new NotificarUsuarioCommand(
                $"Criação de Aula de Reposição na turma {turma.Nome}",
                $"A criação de {aula.Quantidade} aula(s) de reposição de {turma.ModalidadeCodigo.ToString()} na turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Nome}) foi recusada. Motivo: {motivo} ",
                usuario.CodigoRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                turma.Ue?.Dre?.CodigoDre,
                aula.UeId,
                aula.TurmaId,
                aula.CriadoEm.Year,
                codigoDaNotificacao,
                usuarioId: usuario.Id));
        }

        private async Task NotificarCriadorDaAulaQueFoiAprovada(Aula aula, long codigoDaNotificacao)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(aula.TurmaId);
            if (turma.EhNulo())
                throw new NegocioException("Turma não localizada.");

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(aula.CriadoRF);

            await mediator.Send(new NotificarUsuarioCommand(
                $"Criação de Aula de Reposição na turma {turma.Nome} ",
                $"Criação de {aula.Quantidade} aula(s) de reposição de {turma.ModalidadeCodigo.ToString()} na turma {turma.Nome} da {turma.Ue?.Nome} ({turma?.Ue?.Dre?.Nome}) foi aceita.",
                usuario.CodigoRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                turma.Ue?.Dre?.CodigoDre,
                aula.UeId,
                aula.TurmaId,
                aula.CriadoEm.Year,
                codigoDaNotificacao,
                usuarioId: usuario.Id));
        }

        private async Task NotificarCriadorEventoDataPassadaAprovado(Evento evento, long codigoDaNotificacao)
        {
            var escola = repositorioUe.ObterPorCodigo(evento.UeId);
            if (escola.EhNulo())
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/:{evento.Id}/";

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);

            await mediator.Send(new NotificarUsuarioCommand(
                "Criação de evento com data passada",
                $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola} foi aceito. Agora este evento está visível para todos os usuários. Para visualizá-lo clique <a href='{linkParaEvento}'>aqui</a>.",
                usuario.CodigoRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                evento.DreId,
                evento.UeId,
                ano: evento.CriadoEm.Year,
                codigo: codigoDaNotificacao,
                usuarioId: usuario.Id));
        }

        private async Task NotificarCriadorEventoDataPassadaReprovacao(Evento evento, long codigoDaNotificacao, string motivoRecusa)
        {
            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);

            var escola = await repositorioUe.ObterNomePorCodigo(evento.UeId);
            if (string.IsNullOrEmpty(escola))
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            await mediator.Send(new NotificarUsuarioCommand(
                "Criação de evento com data passada",
                $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola} foi recusado. <br/> Motivo: {motivoRecusa}",
                usuario.CodigoRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                evento.DreId,
                evento.UeId,
                ano: evento.CriadoEm.Year,
                codigo: codigoDaNotificacao,
                usuarioId: usuario.Id));
        }

        private async Task NotificarCriadorEventoLiberacaoExcepcionalAprovado(Evento evento, long codigoDaNotificacao)
        {
            var escola = await repositorioUe.ObterNomePorCodigo(evento.UeId);
            if (string.IsNullOrEmpty(escola))
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/:{evento.Id}/";

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);

            await mediator.Send(new NotificarUsuarioCommand(
                "Criação de Eventos Excepcionais",
                $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola} foi aceito. Agora este evento está visível para todos os usuários. Para visualizá-lo clique <a href='{linkParaEvento}'>aqui</a>.",
                usuario.CodigoRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                evento.DreId,
                evento.UeId,
                ano: evento.CriadoEm.Year,
                codigo: codigoDaNotificacao,
                usuarioId: usuario.Id));
        }

        private async Task NotificarDiretorUeEventoDataPassadaAprovado(Evento evento, long codigoDaNotificacao)
        {
            var escola = await repositorioUe.ObterNomePorCodigo(evento.UeId);

            if (string.IsNullOrEmpty(escola))
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            var funcionariosEscola = await servicoNotificacao.ObterFuncionariosPorNivelAsync(evento.UeId, Cargo.Diretor, true, true);

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/{evento.Id}/";

            foreach (var funcionario in funcionariosEscola)
            {
                var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id);

                await mediator.Send(new NotificarUsuarioCommand(
                    "Criação de evento com data passada",
                    $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {escola} foi aceito. Agora este evento está visível para todos os usuários. Para visualizá-lo clique <a href='{linkParaEvento}'>aqui</a>.",
                    usuario.CodigoRf,
                    NotificacaoCategoria.Aviso,
                    NotificacaoTipo.Calendario,
                    evento.DreId,
                    evento.UeId,
                    ano: evento.CriadoEm.Year,
                    codigo: codigoDaNotificacao,
                    usuarioId: usuario.Id));
            }
        }

        private async Task NotificarEventoQueFoiReprovado(Evento evento, long codigoDaNotificacao, Usuario usuario, string motivoRecusa, string nomeEscola)
        {
            await mediator.Send(new NotificarUsuarioCommand(
                "Criação de Eventos Excepcionais",
                $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} do calendário {evento.TipoCalendario.Nome} da {nomeEscola} foi recusado. <br/> Motivo: {motivoRecusa}",
                usuario.CodigoRf,
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.Calendario,
                evento.DreId,
                evento.UeId,
                ano: evento.CriadoEm.Year,
                codigo: codigoDaNotificacao,
                usuarioId: usuario.Id));
        }

        private async Task ReprovarNivel(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou, WorkflowAprovacaoNivel nivel)
        {
            if (workflow.Tipo == WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional)
            {
                await TrataReprovacaoEventoLiberacaoExcepcional(workflow, codigoDaNotificacao, motivo, cargoDoNivelQueRecusou);
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
                await TrataReprovacaoFechamentoReabertura(workflow, codigoDaNotificacao, motivo, nivel.Id);
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
            
            await AtualizarNotasFechamento(notasEmAprovacao);
            
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(workflow.TurmaId);
            
            await NotificarAprovacaoNotasFechamento(notasEmAprovacao, codigoDaNotificacao, turma, false, motivo);
        }

        private async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> ObterNotasEmAprovacao(long workflowId)
            => await mediator.Send(new ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery(workflowId));

        private async Task TrataReprovacaoAlteracaoNotaPosConselho(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            await mediator.Send(new RecusarAprovacaoNotaConselhoCommand(codigoDaNotificacao,
                                                                        workflow.TurmaId,
                                                                        workflow.Id,
                                                                        motivo));
        }

        private void TrataReprovacaoEventoDataPassada(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflow.Id);
            if (evento.EhNulo())
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.ReprovarWorkflow();
            repositorioEvento.Salvar(evento);

            NotificarCriadorEventoDataPassadaReprovacao(evento, codigoDaNotificacao, motivo);
        }

        private async Task TrataReprovacaoEventoLiberacaoExcepcional(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, Cargo? cargoDoNivelQueRecusou)
        {
            Evento evento = repositorioEvento.ObterPorWorkflowId(workflow.Id);
            if (evento.EhNulo())
                throw new NegocioException("Não foi possível localizar o evento deste fluxo de aprovação.");

            evento.ReprovarWorkflow();
            repositorioEvento.Salvar(evento);

            var escola = repositorioUe.ObterPorCodigo(evento.UeId);

            if (escola.EhNulo())
                throw new NegocioException("Não foi possível localizar a Ue deste evento.");

            if (cargoDoNivelQueRecusou == Cargo.Supervisor)
            {
                var funcionariosRetorno = servicoNotificacao.ObterFuncionariosPorNivel(evento.UeId, Cargo.Diretor, true, true);

                foreach (var funcionario in funcionariosRetorno)
                {
                    var usuarioDiretor = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(funcionario.Id);

                    await NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuarioDiretor, motivo, escola.Nome);
                }
            }
            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(evento.CriadoRF);
            await NotificarEventoQueFoiReprovado(evento, codigoDaNotificacao, usuario, motivo, escola.Nome);
        }

        private async Task TrataReprovacaoFechamentoReabertura(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo, long nivelId)
        {
            FechamentoReabertura fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(0, workflow.Id);
            if (fechamentoReabertura.EhNulo())
                throw new NegocioException("Não foi possível localizar a reabertura do fechamento do fluxo de aprovação.");

            fechamentoReabertura.ReprovarWorkFlow();

            repositorioFechamentoReabertura.Salvar(fechamentoReabertura);

            await NotificarCriadorFechamentoReaberturaReprovado(fechamentoReabertura, codigoDaNotificacao, motivo, nivelId);
        }

        private async Task TrataReprovacaoReposicaoAula(WorkflowAprovacao workflow, long codigoDaNotificacao, string motivo)
        {

            Aula aula = await repositorioAula.ObterPorWorkflowId(workflow.Id);
            if (aula.EhNulo())
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