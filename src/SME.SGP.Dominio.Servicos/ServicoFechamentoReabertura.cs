using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoReabertura : IServicoFechamentoReabertura
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IUnitOfWork unitOfWork,
            IComandosWorkflowAprovacao comandosWorkflowAprovacao, IServicoUsuario servicoUsuario, IServicoEOL servicoEOL, IServicoNotificacao servicoNotificacao)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<string> AlterarAsync(FechamentoReabertura fechamentoReabertura, DateTime dataInicialAnterior, DateTime dataFimAnterior)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null);

            var fechamentoReaberturasParaVerificar = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id);
            var fechamentoReaberturasParaAtualizar = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id && fechamentoReabertura.Inicio > a.Inicio || a.Fim > fechamentoReabertura.Fim);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            fechamentoReabertura.PodeSalvar(fechamentoReaberturasParaVerificar, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            var mensagemRetorno = "Reabertura de Fechamento alterado com sucesso.";

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento alterado e será válido após aprovação.";
            }
            else if (fechamentoReabertura.DeveCriarEventos())
            {
                //Criar eventos;
            }

            VerificaEAtualizaFechamentosReaberturasParaAlterar(fechamentoReabertura, fechamentoReaberturasParaAtualizar);

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

                var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null, null);
                var fechamentoReaberturasParaExcluir = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id && fechamentoReabertura.Inicio > a.Inicio || a.Fim > fechamentoReabertura.Fim);

                if (fechamentoReaberturasParaExcluir != null && fechamentoReaberturasParaExcluir.Any())
                {
                    foreach (var fechamentoReaberturaParaExcluir in fechamentoReaberturasParaExcluir)
                    {
                        if (fechamentoReabertura.WorkflowAprovacaoId.HasValue)
                        {
                            await comandosWorkflowAprovacao.ExcluirAsync(fechamentoReaberturaParaExcluir.WorkflowAprovacaoId.Value);
                        }

                        var notificacoesParaExcluir = await repositorioFechamentoReabertura.ObterNotificacoes(fechamentoReaberturaParaExcluir.Id);
                        if (notificacoesParaExcluir != null && notificacoesParaExcluir.Any())
                        {
                            await servicoNotificacao.ExcluirFisicamenteAsync(notificacoesParaExcluir.Select(a => a.NotificacaoId).ToArray());
                        }

                        fechamentoReaberturaParaExcluir.Excluir();
                        await repositorioFechamentoReabertura.SalvarAsync(fechamentoReaberturaParaExcluir);
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

            return string.Empty;
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
                fechamentoReabertura.WorkflowAprovacaoId = PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
                mensagemRetorno = "Reabertura de Fechamento cadastrado e será válido após aprovação.";
            }
            else if (fechamentoReabertura.DeveCriarEventos())
            {
                //Criar eventos;
            }

            unitOfWork.PersistirTransacao();

            return mensagemRetorno;
        }

        private void AtualizaFechamentosComDatasDistintas(FechamentoReabertura fechamentoReabertura, List<(FechamentoReabertura, bool, bool)> fechamentosReaberturasParaAtualizar)
        {
            foreach (var fechamentoReaberturaParaAtualizar in fechamentosReaberturasParaAtualizar)
            {
                repositorioFechamentoReabertura.Salvar(fechamentoReaberturaParaAtualizar.Item1);
                NotificarSobreAlteracaoNoFechamentoReabertura(fechamentoReaberturaParaAtualizar);
            }
        }

        private void NotificarSobreAlteracaoNoFechamentoReabertura((FechamentoReabertura, bool, bool) fechamentoReaberturaParaAtualizar)
        {
            var fechamentoReabertura = fechamentoReaberturaParaAtualizar.Item1;

            var notificacao = new Notificacao()
            {
                UeId = fechamentoReabertura.Ue is null ? null : fechamentoReabertura.Ue.CodigoUe,
                Ano = fechamentoReabertura.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                Titulo = "Alteração em datas de fechamento de bimestre",
                Tipo = NotificacaoTipo.Calendario,
                Mensagem = $@"A {(fechamentoReabertura.EhParaDre() ? "SME" : "Dre")} realizou alterações em datas de reabertura do período de fechamento de bimestre e as datas definidas pela {(fechamentoReabertura.EhParaDre() ? fechamentoReabertura.Dre.Nome : fechamentoReabertura.Ue.Nome)} foram ajustadas. As novas datas são: <br />
                                  { fechamentoReabertura.TipoCalendario.Nome } - { fechamentoReabertura.TipoCalendario.AnoLetivo }
                                  { (fechamentoReaberturaParaAtualizar.Item2 ? " - Nova data de início do período: " + fechamentoReabertura.Inicio.ToString("dd/MM/yyyy") : string.Empty) }
                                  { (fechamentoReaberturaParaAtualizar.Item2 ? " - Nova data de fim do período: " + fechamentoReabertura.Fim.ToString("dd/MM/yyyy") : string.Empty) }"
            };

            if (fechamentoReabertura.EhParaDre())
            {
                var adminsSgpDre = servicoEOL.ObterAdministradoresSGP(fechamentoReabertura.Dre.CodigoDre).Result;
                if (adminsSgpDre != null || adminsSgpDre.Any())
                {
                    foreach (var adminSgpUe in adminsSgpDre)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                        notificacao.UsuarioId = usuario.Id;

                        servicoNotificacao.Salvar(notificacao);
                        repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
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
                        notificacao.UsuarioId = usuario.Id;

                        servicoNotificacao.Salvar(notificacao);
                        repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }

                var diretores = servicoEOL.ObterFuncionariosPorCargoUe(fechamentoReabertura.Ue.CodigoUe, (long)Cargo.Diretor);
                if (diretores == null || !diretores.Any())
                    throw new NegocioException($"Não foram localizados diretores para Ue {fechamentoReabertura.Ue.CodigoUe}.");

                foreach (var diretor in diretores)
                {
                    var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);
                    notificacao.UsuarioId = usuario.Id;

                    servicoNotificacao.Salvar(notificacao);
                    repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                }
            }
        }

        private long PersistirWorkflowFechamentoReabertura(FechamentoReabertura fechamentoReabertura)
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
                NotificacaoMensagem = $@"A {fechamentoReabertura.Ue.Nome} cadastrou um novo período de reabertura de fechamento de bimestre para o tipo de calendário {fechamentoReabertura.TipoCalendario.Nome} do ano de {fechamentoReabertura.TipoCalendario.AnoLetivo}. Para que o período seja considerado válido é necessário que você aceite esta notificação. <br />
                                           Descrição: {fechamentoReabertura.Descricao} <br />
                                           Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br />
                                           Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br />
                                           Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}"
            };

            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Supervisor,
                Nivel = 1
            });

            return comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
        }

        private void VerificaEAtualizaFechamentosReaberturasParaAlterar(FechamentoReabertura fechamentoReabertura, IEnumerable<FechamentoReabertura> fechamentoReaberturas)
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

                    if (atualizaInicio || atualizaFim)
                        fechamentosParaAtualizarTupple.Add((fechamentoReaberturasParaAtualizar, atualizaInicio, atualizaFim));
                }

                AtualizaFechamentosComDatasDistintas(fechamentoReabertura, fechamentosParaAtualizarTupple);
            }
        }
    }
}