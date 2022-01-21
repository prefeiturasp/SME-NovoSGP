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
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null);

            var fechamentoReaberturasParaVerificar = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            fechamentoReabertura.PodeSalvar(fechamentoReaberturasParaVerificar, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            var excluirBimestres = fechamentoReabertura.Bimestres.Select(s => s.Bimestre).Except(bimestresPropostos).ToList();
            foreach (var excluirBimestreId in excluirBimestres)
            {
                repositorioFechamentoReabertura.ExcluirBimestre(fechamentoReabertura.Id, excluirBimestreId);
                fechamentoReabertura.SobrescreverBimestres(fechamentoReabertura.Bimestres.Where(x => x.Bimestre != excluirBimestreId));
            }

            var adicionarBimestres = bimestresPropostos.Except(fechamentoReabertura.Bimestres.Select(s => s.Bimestre)).ToList();
            foreach (var adicionarBimestreId in adicionarBimestres)
            {
                var novoBimestre = new FechamentoReaberturaBimestre
                {
                    FechamentoAberturaId = fechamentoReaberturaId,
                    CriadoPor = usuarioAtual.Nome,
                    CriadoRF = usuarioAtual.CodigoRf,
                    Bimestre = adicionarBimestreId,
                };
                await repositorioFechamentoReabertura.SalvarBimestreAsync(novoBimestre);
                fechamentoReabertura.Adicionar(novoBimestre);
            }

            var mensagemRetorno = "Reabertura de Fechamento alterado com sucesso.";

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = await PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento alterado e será válido após aprovação.";
            }
            else
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReabertura, MapearFechamentoReaberturaNotificacao(fechamentoReabertura, usuarioAtual), new System.Guid(), usuarioAtual));

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
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

        public async Task<string> ExcluirAsync(FechamentoReabertura fechamentoReabertura)
        {
            var fechamentoReaberturasParaExcluir = Enumerable.Empty<FechamentoReabertura>();
            if (fechamentoReabertura.EhParaUe())
            {
                var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, fechamentoReabertura.DreId, fechamentoReabertura.UeId, null);
                fechamentoReaberturasParaExcluir = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id && fechamentoReabertura.Inicio >= a.Inicio || a.Fim >= fechamentoReabertura.Fim);
            }

            unitOfWork.IniciarTransacao();
            try
            {
                fechamentoReabertura.Excluir();
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

                if (fechamentoReabertura.EhParaUe())
                    await ExcluirVinculosAysnc(fechamentoReabertura);
                else
                {
                    if (fechamentoReaberturasParaExcluir != null && fechamentoReaberturasParaExcluir.Any())
                        foreach (var fechamentoReaberturaParaExcluir in fechamentoReaberturasParaExcluir)
                            await ExcluirVinculosAysnc(fechamentoReaberturaParaExcluir);
                }
                unitOfWork.PersistirTransacao();
            }
            catch (NegocioException nEx)
            {
                unitOfWork.Rollback();
                return nEx.Message;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                return $"Não foi possível excluir o fechamento de reabertura de código {fechamentoReabertura.Id}";
            }

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
                fechamentoReabertura.Bimestres.ToList().ForEach(f => f.FechamentoAbertura = null);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReabertura, MapearFechamentoReaberturaNotificacao(fechamentoReabertura, usuarioAtual), new System.Guid(), usuarioAtual));
            }

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
        }
        
        private async Task ExcluirVinculosAysnc(FechamentoReabertura fechamentoReaberturaParaExcluir)
        {
            if (fechamentoReaberturaParaExcluir.WorkflowAprovacaoId.HasValue)
                await comandosWorkflowAprovacao.ExcluirAsync(fechamentoReaberturaParaExcluir.WorkflowAprovacaoId.Value);

            var notificacoesParaExcluir = await repositorioFechamentoReabertura.ObterNotificacoes(fechamentoReaberturaParaExcluir.Id);
            if (notificacoesParaExcluir != null && notificacoesParaExcluir.Any())
            {
                await repositorioFechamentoReabertura.ExcluirVinculoDeNotificacoesAsync(fechamentoReaberturaParaExcluir.Id);
                await servicoNotificacao.ExcluirFisicamenteAsync(notificacoesParaExcluir.Select(a => a.NotificacaoId).ToArray());
            }
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