﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoCadastroFechamentoReaberturaCommandHandler : IRequestHandler<ExecutaNotificacaoCadastroFechamentoReaberturaCommand, bool>
    {
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IMediator mediator;

        public ExecutaNotificacaoCadastroFechamentoReaberturaCommandHandler(IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IServicoNotificacao servicoNotificacao, IMediator mediator)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoCadastroFechamentoReaberturaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioPorRfOuCriaQuery(request.FechamentoReabertura.CodigoRf));
            var notificacao = await CriaNotificacaoCadastro(request.FechamentoReabertura, usuario.Id);
            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = request.FechamentoReabertura.Id, NotificacaoId = notificacao.Id });

            return true;
        }

        private async Task<Notificacao> CriaNotificacaoCadastro(FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura, long usuarioId)
        {
            var tituloNotificacao = $"Período de reabertura - {fechamentoReabertura.TipoCalendarioNome}";

            string descricaoDreUe;
            if (fechamentoReabertura.EhParaUe)
            {
                var descricaoBase = $"{fechamentoReabertura.UeNome} (DRE {fechamentoReabertura.DreAbreviacao})";
                descricaoDreUe = $"a {descricaoBase})";
                tituloNotificacao += $" - {descricaoBase}";
            }
            else
                descricaoDreUe = "todas as DREs/UEs";

            string notificacaoMensagem = $@"Um novo período de reabertura foi cadastrado para {descricaoDreUe} <br/>
                                           Tipo de calendário: {fechamentoReabertura.TipoCalendarioNome} <br/>
                                           Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br/>
                                           Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br/>
                                           Bimestres: {fechamentoReabertura.Bimestres}";

            var notificacao = new Notificacao()
            {
                UeId = fechamentoReabertura.UeCodigo,
                Ano = fechamentoReabertura.AnoLetivo,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = fechamentoReabertura.DreCodigo,
                Titulo = tituloNotificacao,
                Tipo = NotificacaoTipo.Calendario,
                UsuarioId = usuarioId,
                Mensagem = notificacaoMensagem
            };

            await servicoNotificacao.Salvar(notificacao);

            return notificacao;
        }
    }
}
