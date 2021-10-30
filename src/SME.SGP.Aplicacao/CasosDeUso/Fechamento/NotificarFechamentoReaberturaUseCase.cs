using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarFechamentoReaberturaUseCase : AbstractUseCase, INotificarFechamentoReaberturaUseCase
    {
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoNotificacao servicoNotificacao;
        public NotificarFechamentoReaberturaUseCase(IMediator mediator, IServicoUsuario servicoUsuario,
                                                    IServicoEol servicoEOL, IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IServicoNotificacao servicoNotificacao) : base(mediator)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroFechamentoReaberturaNotificacaoDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados", LogNivel.Informacao, LogContexto.Fechamento));
                return false;
            }
            else
            {
                if (filtro.FechamentoReabertura.EhParaSme()) //todas as DRE's e UE's
                {
                    var verificarUesTipoCalendario = await mediator.Send(new ObterGestoresDreUePorTipoCalendarioModalidadeQuery(filtro.FechamentoReabertura.TipoCalendario.Modalidade, filtro.FechamentoReabertura.TipoCalendario.AnoLetivo));
                    var agrupamentoUeporDre = verificarUesTipoCalendario.GroupBy(d => d.Dre).ToDictionary(group => group.Key, group => group.ToList().Select(s => s.Ue));

                    foreach (var valores in agrupamentoUeporDre)
                    {
                       await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReaberturaSME, new FiltroNotificacaoFechamentoReaberturaSMEDto(valores.Key, valores.Value, filtro.FechamentoReabertura), new System.Guid(), filtro.Usuario));
                    }
                }
                else if (filtro.FechamentoReabertura.EhParaUe())
                {
                    var fechamentoReabertura = filtro.FechamentoReabertura;

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
                    if (cps != null && cps.Any())
                    {
                        foreach (var cp in cps)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cp.CodigoRf);

                            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, null, null);

                            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                        }
                    }
                }

                return true;
            }
        }

        private Notificacao CriaNotificacaoCadastro(FechamentoReabertura fechamentoReabertura, long usuarioId, string dreCodigo, string ueCodigo)
        {
            var tituloNotificacao = $"Período de reabertura - {fechamentoReabertura.TipoCalendario.Nome}";
            string descricaoBase, descricaoDreUe;

            if (dreCodigo == null && ueCodigo == null)
            {
                dreCodigo = fechamentoReabertura.Dre.CodigoDre;
                ueCodigo = fechamentoReabertura.Ue.CodigoUe;
            }

            if (fechamentoReabertura.Ue == null && fechamentoReabertura.Dre == null)
                descricaoDreUe = "todas as DREs/UEs";
            else
            {
                descricaoBase = $"{fechamentoReabertura.Ue.Nome} (DRE {fechamentoReabertura.Dre.Abreviacao})";
                descricaoDreUe = $"a {descricaoBase})";
                tituloNotificacao += $" - {descricaoBase}";
            }

            string notificacaoMensagem = $@"Um novo periodo de reabertura foi cadastrado para {descricaoDreUe} <br/>
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
                Titulo = tituloNotificacao,
                Tipo = NotificacaoTipo.Calendario,
                UsuarioId = usuarioId,
                Mensagem = notificacaoMensagem
            };

            servicoNotificacao.Salvar(notificacao);

            return notificacao;
        }
    }
}
