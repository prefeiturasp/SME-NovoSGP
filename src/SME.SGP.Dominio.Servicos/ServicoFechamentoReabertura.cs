using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoReabertura : IServicoFechamentoReabertura
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IUnitOfWork unitOfWork,
            IComandosWorkflowAprovacao comandosWorkflowAprovacao, IServicoUsuario servicoUsuario, IServicoEol servicoEOL, IServicoNotificacao servicoNotificacao,
            IRepositorioEventoTipo repositorioEventoTipo, IServicoEvento servicoEvento, IRepositorioEvento repositorioEvento, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<string> AlterarAsync(FechamentoReabertura fechamentoReabertura, DateTime dataInicialAnterior, DateTime dataFimAnterior, bool confirmacacaoAlteracaoHierarquica)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null);

            var fechamentoReaberturasParaVerificar = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id);
            var fechamentoReaberturasParaAtualizar = fechamentoReaberturasParaVerificar.Where(a => fechamentoReabertura.Inicio > a.Inicio || a.Fim > fechamentoReabertura.Fim);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            fechamentoReabertura.PodeSalvar(fechamentoReaberturasParaVerificar, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            var mensagemRetorno = "Reabertura de Fechamento alterado com sucesso.";

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = await PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento alterado e será válido após aprovação.";
            }

            await VerificaEAtualizaFechamentosReaberturasParaAlterar(fechamentoReabertura, fechamentoReaberturasParaAtualizar, confirmacacaoAlteracaoHierarquica);

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
        }

        public async Task<string> ExcluirAsync(FechamentoReabertura fechamentoReabertura)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                fechamentoReabertura.Excluir();
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

                if (fechamentoReabertura.EhParaUe())
                {
                    await ExcluirVinculosAysnc(fechamentoReabertura);
                }
                else
                {
                    var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null);
                    var fechamentoReaberturasParaExcluir = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id && fechamentoReabertura.Inicio >= a.Inicio || a.Fim >= fechamentoReabertura.Fim);

                    if (fechamentoReaberturasParaExcluir != null && fechamentoReaberturasParaExcluir.Any())
                    {
                        foreach (var fechamentoReaberturaParaExcluir in fechamentoReaberturasParaExcluir)
                        {
                            await ExcluirVinculosAysnc(fechamentoReaberturaParaExcluir);
                        }
                    }
                }
            }
            catch (NegocioException nEx)
            {
                return nEx.Message;
            }
            catch (Exception)
            {
                return $"Não foi possível excluir o fechamento de reabertura de código {fechamentoReabertura.Id}";
            }

            unitOfWork.PersistirTransacao();

            return "Exclusão efetuada com sucesso.";
        }

        public async Task<string> SalvarAsync(FechamentoReabertura fechamentoReabertura)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();
            fechamentoReabertura.PodeSalvar(fechamentoReaberturas, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            var mensagemRetorno = "Reabertura de Fechamento cadastrada com sucesso";

            foreach (var fechamentoReaberturaBimestre in fechamentoReabertura.Bimestres)
            {
                fechamentoReaberturaBimestre.FechamentoAberturaId = fechamentoReaberturaId;
                await repositorioFechamentoReabertura.SalvarBimestreAsync(fechamentoReaberturaBimestre);
            }

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = await PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento cadastrado e será válido após aprovação.";
            }
            else if (fechamentoReabertura.DeveCriarEventos())
            {
                await GeraEventos(fechamentoReabertura);
            }

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
        }

        private async Task AtualizaFechamentosComDatasDistintas(List<(FechamentoReabertura, bool, bool)> fechamentosReaberturasParaAtualizar)
        {
            foreach (var fechamentoReaberturaParaAtualizar in fechamentosReaberturasParaAtualizar)
            {
                repositorioFechamentoReabertura.Salvar(fechamentoReaberturaParaAtualizar.Item1);
                await NotificarSobreAlteracaoNoFechamentoReabertura(fechamentoReaberturaParaAtualizar);
            }
        }

        private async Task AtualizoEvento(FechamentoReabertura fechamentoReaberturasParaAtualizar, DateTime inicio, DateTime fim)
        {
            var eventosParaAtualizar = await repositorioEvento.EventosNosDiasETipo(fechamentoReaberturasParaAtualizar.Inicio, fechamentoReaberturasParaAtualizar.Fim, TipoEvento.FechamentoBimestre, fechamentoReaberturasParaAtualizar.TipoCalendarioId, fechamentoReaberturasParaAtualizar.Ue.CodigoUe, fechamentoReaberturasParaAtualizar.Dre.CodigoDre, false);
            if (eventosParaAtualizar != null && eventosParaAtualizar.Any())
            {
                var eventoParaAtualizar = eventosParaAtualizar.FirstOrDefault();
                eventoParaAtualizar.DataInicio = inicio;
                eventoParaAtualizar.DataFim = fim;
                await servicoEvento.Salvar(eventoParaAtualizar, false, false, true);
            }
        }

        private async Task ExcluirVinculosAysnc(FechamentoReabertura fechamentoReaberturaParaExcluir)
        {
            if (fechamentoReaberturaParaExcluir.WorkflowAprovacaoId.HasValue)
            {
                await comandosWorkflowAprovacao.ExcluirAsync(fechamentoReaberturaParaExcluir.WorkflowAprovacaoId.Value);
            }

            var notificacoesParaExcluir = await repositorioFechamentoReabertura.ObterNotificacoes(fechamentoReaberturaParaExcluir.Id);
            if (notificacoesParaExcluir != null && notificacoesParaExcluir.Any())
            {
                await repositorioFechamentoReabertura.ExcluirVinculoDeNotificacoesAsync(fechamentoReaberturaParaExcluir.Id);
                await servicoNotificacao.ExcluirFisicamenteAsync(notificacoesParaExcluir.Select(a => a.NotificacaoId).ToArray());
            }
            if (fechamentoReaberturaParaExcluir.EhParaUe())
            {
                var eventosParaExcluir = await repositorioEvento.EventosNosDiasETipo(fechamentoReaberturaParaExcluir.Inicio, fechamentoReaberturaParaExcluir.Fim, TipoEvento.FechamentoBimestre, fechamentoReaberturaParaExcluir.TipoCalendarioId, fechamentoReaberturaParaExcluir.Ue.CodigoUe, fechamentoReaberturaParaExcluir.Dre.CodigoDre, false);
                if (eventosParaExcluir != null && eventosParaExcluir.Any())
                {
                    var eventoParaExcluir = eventosParaExcluir.FirstOrDefault();
                    servicoEvento.Excluir(eventoParaExcluir);
                }
            }
        }

        private async Task GeraEventos(FechamentoReabertura fechamentoReabertura)
        {
            EventoTipo tipoEvento = ObterTipoEvento();

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
            };

            await servicoEvento.Salvar(evento, false, false, true);
        }

        private async Task NotificarSobreAlteracaoNoFechamentoReabertura((FechamentoReabertura, bool, bool) fechamentoReaberturaParaAtualizar)
        {
            var fechamentoReabertura = fechamentoReaberturaParaAtualizar.Item1;

            if (fechamentoReabertura.EhParaDre())
            {
                var adminsSgpDre = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Dre.CodigoDre).Result;
                if (adminsSgpDre != null && adminsSgpDre.Any())
                {
                    foreach (var adminSgpUe in adminsSgpDre)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                        var notificacao = CriaNovaNotificacao(fechamentoReaberturaParaAtualizar, fechamentoReabertura, usuario.Id);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
            }
            else if (fechamentoReabertura.EhParaUe())
            {
                var adminsSgpUe = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Ue.CodigoUe).Result;

                if (adminsSgpUe != null && adminsSgpUe.Any())
                {
                    foreach (var adminSgpUe in adminsSgpUe)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);

                        var notificacao = CriaNovaNotificacao(fechamentoReaberturaParaAtualizar, fechamentoReabertura, usuario.Id);

                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }

                var diretores = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);

                if (diretores != null && diretores.Any())
                {
                    foreach (var diretor in diretores)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);

                        var notificacao = CriaNovaNotificacao(fechamentoReaberturaParaAtualizar, fechamentoReabertura, usuario.Id);

                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
                else
                {
                    var ads = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.AD);
                    if (ads != null && ads.Any())
                    {
                        foreach (var ad in ads)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(ad.CodigoRf);

                            var notificacao = CriaNovaNotificacao(fechamentoReaberturaParaAtualizar, fechamentoReabertura, usuario.Id);

                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
                    }
                    else
                    {
                        var supervisor = repositorioSupervisorEscolaDre.ObtemPorUe(fechamentoReabertura.Ue.CodigoUe);
                        if (supervisor != null)
                        {

                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(supervisor.SupervisorId);

                            var notificacao = CriaNovaNotificacao(fechamentoReaberturaParaAtualizar, fechamentoReabertura, usuario.Id);

                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });

                        }
                        else
                        {
                            var supervisoresTecnicos = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.SupervisorTecnico);
                            if (supervisoresTecnicos != null && supervisoresTecnicos.Any())
                            {
                                foreach (var supervisoresTecnico in supervisoresTecnicos)
                                {
                                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(supervisoresTecnico.CodigoRf);

                                    var notificacao = CriaNovaNotificacao(fechamentoReaberturaParaAtualizar, fechamentoReabertura, usuario.Id);

                                    await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                                }
                            }
                        }

                    }

                }


            }
        }

        private Notificacao CriaNovaNotificacao((FechamentoReabertura, bool, bool) fechamentoReaberturaParaAtualizar, FechamentoReabertura fechamentoReabertura, long usuarioId)
        {
            var notificacao = new Notificacao()
            {
                UeId = fechamentoReabertura.Ue is null ? null : fechamentoReabertura.Ue.CodigoUe,
                Ano = fechamentoReabertura.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                Titulo = "Alteração em datas de fechamento de bimestre",
                Tipo = NotificacaoTipo.Calendario,
                UsuarioId = usuarioId,
                Mensagem = $@"A SME realizou alterações em datas de reabertura do período de fechamento de bimestre para os bimestres
                                 { fechamentoReabertura.ObterBimestresNumeral()} e as datas definidas pela <b>{(fechamentoReabertura.EhParaDre() ? fechamentoReabertura.Dre.Nome
                                : $@"{fechamentoReabertura.Ue.TipoEscola.ShortName()} {fechamentoReabertura.Ue.Nome}")}</b> foram ajustadas. As novas datas são: <br/>
                                  { fechamentoReabertura.TipoCalendario.Nome } - { fechamentoReabertura.TipoCalendario.AnoLetivo }   
                                  { (fechamentoReaberturaParaAtualizar.Item2 ? " - Nova data de início do período: " + fechamentoReabertura.Inicio.ToString("dd/MM/yyyy") : string.Empty) }
                                  { (fechamentoReaberturaParaAtualizar.Item3 ? " - Nova data de fim do período: " + fechamentoReabertura.Fim.ToString("dd/MM/yyyy") : string.Empty) }"
            };

            servicoNotificacao.Salvar(notificacao);

            return notificacao;
        }

        private EventoTipo ObterTipoEvento()
        {
            EventoTipo tipoEvento = repositorioEventoTipo.ObterPorCodigo((int)TipoEvento.FechamentoBimestre);
            if (tipoEvento == null)
                throw new NegocioException($"Não foi possível localizar o tipo de evento {TipoEvento.FechamentoBimestre.GetAttribute<DisplayAttribute>().Name}.");
            return tipoEvento;
        }

        private async Task<long> PersistirWorkflowFechamentoReabertura(FechamentoReabertura fechamentoReabertura)
        {
            var wfAprovacaoEvento = new WorkflowAprovacaoDto()
            {
                Ano = fechamentoReabertura.Inicio.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = fechamentoReabertura.Id,
                Tipo = WorkflowAprovacaoTipo.Fechamento_Reabertura,
                UeId = fechamentoReabertura.Ue.CodigoUe,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                NotificacaoTitulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $@"A {fechamentoReabertura.Ue.TipoEscola.ShortName()} {fechamentoReabertura.Ue.Nome}({fechamentoReabertura.Dre.Abreviacao}) cadastrou um novo período de reabertura de fechamento de bimestre para o tipo de calendário <b>{fechamentoReabertura.TipoCalendario.Nome}</b> do ano de {fechamentoReabertura.TipoCalendario.AnoLetivo}. Para que o período seja considerado válido é necessário que você aceite esta notificação. <br/>
                                           Descrição: {fechamentoReabertura.Descricao} <br/>
                                           Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                                           Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                                           Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}"
            };

            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Supervisor,
                Nivel = 1
            });

            return await comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
        }

        private async Task VerificaEAtualizaFechamentosReaberturasParaAlterar(FechamentoReabertura fechamentoReabertura, IEnumerable<FechamentoReabertura> fechamentoReaberturas, bool confirmacacaoAlteracaoHierarquica)
        {
            if (fechamentoReabertura.EhParaDre() || fechamentoReabertura.EhParaSme())
            {
                var fechamentosParaAtualizarTupple = new List<(FechamentoReabertura, bool, bool)>();
                var fechamentosParaAtualizar = new List<FechamentoReabertura>();

                if (fechamentoReabertura.EhParaDre())
                {
                    fechamentosParaAtualizar = fechamentoReaberturas.Where(a => a.EhParaUe() && a.DreId == fechamentoReabertura.DreId).ToList();
                }
                else if (fechamentoReabertura.EhParaSme())
                {
                    fechamentosParaAtualizar = fechamentoReaberturas.Where(a => !a.EhParaSme()).ToList();
                }

                foreach (var fechamentoReaberturasParaAtualizar in fechamentosParaAtualizar)
                {
                    var atualizaInicio = false;
                    var atualizaFim = false;

                    if (fechamentoReaberturasParaAtualizar.Inicio < fechamentoReabertura.Inicio)
                    {
                        fechamentoReaberturasParaAtualizar.Inicio = fechamentoReabertura.Inicio;
                        atualizaInicio = true;
                    }

                    if (fechamentoReaberturasParaAtualizar.Fim > fechamentoReabertura.Fim)
                    {
                        fechamentoReaberturasParaAtualizar.Fim = fechamentoReabertura.Fim;
                        atualizaFim = true;
                    }
                    if (fechamentoReaberturasParaAtualizar.EhParaUe())
                    {
                        await AtualizoEvento(fechamentoReaberturasParaAtualizar, fechamentoReabertura.Inicio, fechamentoReabertura.Fim);
                    }

                    if (atualizaInicio || atualizaFim)
                        fechamentosParaAtualizarTupple.Add((fechamentoReaberturasParaAtualizar, atualizaInicio, atualizaFim));
                }
                if (fechamentosParaAtualizarTupple.Any())
                {
                    if (confirmacacaoAlteracaoHierarquica)
                    {
                        await AtualizaFechamentosComDatasDistintas(fechamentosParaAtualizarTupple);
                    }
                    else
                    {
                        var temAlteracoesParaDre = fechamentosParaAtualizarTupple.Any(a => a.Item1.EhParaDre());
                        var temAlteracoesParaUe = fechamentosParaAtualizarTupple.Any(a => a.Item1.EhParaUe());
                        var exibeDreUes = temAlteracoesParaDre && temAlteracoesParaUe;
                        var textoParaExibir = $"{(temAlteracoesParaDre ? "DRE's" : string.Empty)} {(exibeDreUes ? " - " : string.Empty)} {(temAlteracoesParaUe ? "UE's" : string.Empty)}";

                        throw new NegocioException($"A alteração que você está fazendo afetará datas de fechamento definidas pelas {textoParaExibir.Trim()}. Deseja Continuar?", 602);
                    }
                }
            }
            else if (fechamentoReabertura.EhParaUe())
            {
                await AtualizoEvento(fechamentoReabertura, fechamentoReabertura.Inicio, fechamentoReabertura.Fim);
            }
        }
    }
}