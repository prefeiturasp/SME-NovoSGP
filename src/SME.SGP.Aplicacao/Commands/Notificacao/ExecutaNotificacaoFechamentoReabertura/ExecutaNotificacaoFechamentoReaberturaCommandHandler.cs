using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFechamentoReaberturaCommandHandler : IRequestHandler<ExecutaNotificacaoFechamentoReaberturaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoNotificacao servicoNotificacao;

        public ExecutaNotificacaoFechamentoReaberturaCommandHandler(IMediator mediator, IServicoEol servicoEOL, IServicoUsuario servicoUsuario, 
                                                                    IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IServicoNotificacao servicoNotificacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<bool> Handle(ExecutaNotificacaoFechamentoReaberturaCommand request, CancellationToken cancellationToken)
        {
            var dreCodigo = request.DreCodigo;
            var ues = request.Ues;
            var fechamentoReabertura = request.FechamentoReabertura;
            var EhParaSme = fechamentoReabertura.EhParaSme();

            if (EhParaSme)
            {
                var adminsSgpDre = await servicoEOL.ObterAdministradoresSGP(dreCodigo);

                if (adminsSgpDre != null && adminsSgpDre.Any())
                {
                    foreach (var adminSgpUe in adminsSgpDre)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, null, dreCodigo, EhParaSme);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
            }

            foreach (var ue in ues)
            {
                var adminsSgpUe = await servicoEOL.ObterAdministradoresSGP(ue);
                if (adminsSgpUe != null && adminsSgpUe.Any())
                {
                    foreach (var adminSgpUe in adminsSgpUe)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, ue, dreCodigo, EhParaSme);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }

                var diretores = servicoEOL.ObterFuncionariosPorCargoUe(ue, (long)Cargo.Diretor);
                if (diretores != null && diretores.Any())
                {
                    foreach (var diretor in diretores)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);
                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, ue, dreCodigo, EhParaSme);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
                var ads = servicoEOL.ObterFuncionariosPorCargoUe(ue, (long)Cargo.AD);
                if (ads != null && ads.Any())
                {
                    foreach (var ad in ads)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(ad.CodigoRf);
                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, ue, dreCodigo, EhParaSme);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
                var cps = servicoEOL.ObterFuncionariosPorCargoUe(ue, (long)Cargo.CP);
                if (cps != null && cps.Any())
                {
                    foreach (var cp in cps)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cp.CodigoRf);
                        var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, ue, dreCodigo, EhParaSme);
                        await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });
                    }
                }
            }
            return true;
        }

        private Notificacao CriaNotificacaoCadastro(FechamentoReabertura fechamentoReabertura, long usuarioId, string dreCodigo, string ueCodigo, bool ehParaSme)
        {
            var tituloNotificacao = $"Período de reabertura - {fechamentoReabertura.TipoCalendario.Nome}";

            if (!ehParaSme)
            {
                dreCodigo = fechamentoReabertura.Dre.CodigoDre;
                ueCodigo = fechamentoReabertura.Ue.CodigoUe;
            }

            string descricaoDreUe;
            if (fechamentoReabertura.Ue == null && fechamentoReabertura.Dre == null)
                descricaoDreUe = "todas as DREs/UEs";
            else
            {
                descricaoDreUe = $"a {fechamentoReabertura.Ue.Nome} (DRE {fechamentoReabertura.Dre.Abreviacao})";
                tituloNotificacao += $" - (DRE {fechamentoReabertura.Dre.Abreviacao})";
            }

            string notificacaoMensagem = $@"Um novo período de reabertura foi cadastrado para {descricaoDreUe} <br/>
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
