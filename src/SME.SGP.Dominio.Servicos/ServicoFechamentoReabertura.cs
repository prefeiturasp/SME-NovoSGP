using MediatR;
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
        private readonly IMediator mediator;

        public ServicoFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IUnitOfWork unitOfWork,
            IComandosWorkflowAprovacao comandosWorkflowAprovacao, IServicoUsuario servicoUsuario, IServicoEol servicoEOL, IServicoNotificacao servicoNotificacao,
            IRepositorioEventoTipo repositorioEventoTipo, IServicoEvento servicoEvento, IRepositorioEvento repositorioEvento, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre, IMediator mediator)
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
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> AlterarAsync(FechamentoReabertura fechamentoReabertura, int[] bimestresPropostos)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null, null);

            var fechamentoReaberturasParaVerificar = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            fechamentoReabertura.PodeSalvar(fechamentoReaberturasParaVerificar, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            var excluirBimestres = fechamentoReabertura.Bimestres.Select(s => s.Bimestre).Except(bimestresPropostos).ToList();
            foreach (var excluirBimestreId in excluirBimestres)
                repositorioFechamentoReabertura.ExcluirBimestre(fechamentoReabertura.Id, excluirBimestreId);

            var adicionarBimestres = bimestresPropostos.Except(fechamentoReabertura.Bimestres.Select(s => s.Bimestre)).ToList();
            foreach (var adicionarBimestreId in adicionarBimestres)
            {
                await repositorioFechamentoReabertura.SalvarBimestreAsync(new FechamentoReaberturaBimestre
                {
                    FechamentoAberturaId = fechamentoReaberturaId,
                    CriadoPor = usuarioAtual.Nome,
                    CriadoRF = usuarioAtual.CodigoRf,
                    Bimestre = adicionarBimestreId,
                });
            }

            var mensagemRetorno = "Reabertura de Fechamento alterado com sucesso.";

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = await PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento alterado e será válido após aprovação.";
            }
            else
            {
                await NotificarCadastroFechamentoReabertura(fechamentoReabertura);
            }

            //Notificações e WF
            //await NotificarSobreAlteracaoNoFechamentoReabertura(fechamentoReabertura);

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
        }

        public async Task<string> ExcluirAsync(FechamentoReabertura fechamentoReabertura)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                if (fechamentoReabertura.EhParaSme())
                {
                    var fechamentosSME = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendario.Id, null, null, null, null);

                    if (fechamentosSME.Any(f => f.EhParaDre() || f.EhParaUe()))
                        return $"Não foi possível excluir o fechamento de reabertura de código {fechamentoReabertura.Id}, existem fechamentos para DRE/UE relacionados a essa SME";
                }
                else if (fechamentoReabertura.EhParaDre())
                {
                    var fechamentosDre = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendario.Id, fechamentoReabertura.DreId, null, null, null);

                    if (fechamentosDre.Any(f => f.EhParaUe()))
                        return $"Não foi possível excluir o fechamento de reabertura de código {fechamentoReabertura.Id}, existem fechamentos para UE relacionados a essa DRE";
                }

                fechamentoReabertura.Excluir();
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

                if (fechamentoReabertura.EhParaUe())
                {
                    await ExcluirVinculosAysnc(fechamentoReabertura);
                }
                else
                {
                    var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null, null);
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
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null, null);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();
            fechamentoReabertura.PodeSalvar(fechamentoReaberturas, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            var mensagemRetorno = "Reabertura de Fechamento cadastrada com sucesso";

            foreach (var fechamentoReaberturaBimestre in fechamentoReabertura.Bimestres)
            {
                fechamentoReaberturaBimestre.FechamentoAberturaId = fechamentoReaberturaId;

                if (fechamentoReaberturaBimestre.Id > 0)
                {
                    fechamentoReaberturaBimestre.AlteradoEm = DateTime.Now;
                    fechamentoReaberturaBimestre.AlteradoPor = usuarioAtual.Nome;
                    fechamentoReaberturaBimestre.AlteradoRF = usuarioAtual.CodigoRf;
                }
                else
                {
                    fechamentoReaberturaBimestre.CriadoPor = usuarioAtual.Nome;
                    fechamentoReaberturaBimestre.CriadoRF = usuarioAtual.CodigoRf;
                }

                await repositorioFechamentoReabertura.SalvarBimestreAsync(fechamentoReaberturaBimestre);
            }

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = await PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento cadastrado e será válido após aprovação.";
            }
            else
            {
                await NotificarCadastroFechamentoReabertura(fechamentoReabertura);
            }

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
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
                    await servicoEvento.Excluir(eventoParaExcluir);
                }
            }
        }

        private async Task NotificarCadastroFechamentoReabertura(FechamentoReabertura fechamentoReabertura)
        {
            if (fechamentoReabertura.EhParaSme()) //todas as DRE's e UE's
            {
                var verificarUesTipoCalendario = await mediator.Send(new ObterGestoresDreUePorTipoCalendarioModalidadeQuery(fechamentoReabertura.TipoCalendario.Modalidade, fechamentoReabertura.TipoCalendario.AnoLetivo));

                foreach (var valores in verificarUesTipoCalendario)
                {
                    var adminsSgpUe = servicoEOL.ObterAdministradoresSGP(valores.Ue).Result;
                    if (adminsSgpUe != null && adminsSgpUe.Any())
                    {
                        foreach (var adminSgpUe in adminsSgpUe)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, valores.Ue, valores.Dre);
                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
                    }

                    var adminsSgpDre = servicoEOL.ObterAdministradoresSGP(valores.Dre).Result;
                    if (adminsSgpDre != null && adminsSgpDre.Any())
                    {
                        foreach (var adminSgpUe in adminsSgpDre)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, valores.Ue, valores.Dre);
                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
                    }

                    var diretores = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);
                    if (diretores != null && diretores.Any())
                    {
                        foreach (var diretor in diretores)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);

                            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, valores.Ue, valores.Dre);

                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
                    }
                    var ads = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.AD);
                    if (ads != null && ads.Any())
                    {
                        foreach (var ad in ads)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(ad.CodigoRf);

                            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, valores.Ue, valores.Dre);

                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
                    }
                    var cps = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.CP);
                    if (ads != null && ads.Any())
                    {
                        foreach (var cp in cps)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cp.CodigoRf);

                            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, valores.Ue, valores.Dre);

                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
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
                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, null, null);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }

                var diretores = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);
                if (diretores != null && diretores.Any())
                {
                    foreach (var diretor in diretores)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);

                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, null, null);

                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
                var ads = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.AD);
                if (ads != null && ads.Any())
                {
                    foreach (var ad in ads)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(ad.CodigoRf);

                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, null, null);

                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
                var cps = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.CP);
                if (ads != null && ads.Any())
                {
                    foreach (var cp in cps)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cp.CodigoRf);

                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id,null,null);

                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
            }
        }

        private Notificacao CriaNotificacaoCadastro(FechamentoReabertura fechamentoReabertura, long usuarioId, string dreCodigo, string ueCodigo)
        {
            string descricaoDreUe = string.Empty;

            if(dreCodigo == null && ueCodigo == null)
            {
                dreCodigo = fechamentoReabertura.Dre.CodigoDre;
                ueCodigo = fechamentoReabertura.Ue.CodigoUe;
            }

            if (fechamentoReabertura.Ue == null && fechamentoReabertura.Dre == null)
                descricaoDreUe = "todas as DREs/UEs";
            else
            {
                descricaoDreUe = $"a {fechamentoReabertura.Ue.Nome} (DRE {fechamentoReabertura.Dre.Abreviacao})";
            }

            string notificacaoMensagem = $@"Foi cadastrado um novo período de reabertura de fechamento de bimestre para o tipo de calendário <b>{fechamentoReabertura.TipoCalendario.Nome}</b> do ano de {fechamentoReabertura.TipoCalendario.AnoLetivo}. Para que o período seja considerado válido é necessário que você aceite esta notificação. <br/>
                                           Descrição: Um novo periodo de reabertura foi cadastrado para {descricaoDreUe} <br/>
                                           Tipo de calendário: {fechamentoReabertura.TipoCalendario.Nome} <br/>
                                           Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                                           Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                                           Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}";

            var notificacao = new Notificacao()
            {
                UeId = ueCodigo,
                Ano = fechamentoReabertura.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = dreCodigo,
                Titulo = $"Período de reabertura - {fechamentoReabertura.TipoCalendario.Nome}",
                Tipo = NotificacaoTipo.Calendario,
                UsuarioId = usuarioId,
                Mensagem = notificacaoMensagem
            };

            servicoNotificacao.Salvar(notificacao);

            return notificacao;
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
                NotificacaoMensagem = $@"A {fechamentoReabertura.Ue.TipoEscola.ObterNomeCurto()} {fechamentoReabertura.Ue.Nome}({fechamentoReabertura.Dre.Abreviacao}) cadastrou um novo período de reabertura de fechamento de bimestre para o tipo de calendário <b>{fechamentoReabertura.TipoCalendario.Nome}</b> do ano de {fechamentoReabertura.TipoCalendario.AnoLetivo}. Para que o período seja considerado válido é necessário que você aceite esta notificação. <br/>
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

            try
            {
                return await comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}