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
    public class ExecutaNotificacaoCadastroFechamentoReaberturaCommandHandler : IRequestHandler<ExecutaNotificacaoCadastroFechamentoReaberturaCommand, bool>
    {
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoNotificacao servicoNotificacao;

        public ExecutaNotificacaoCadastroFechamentoReaberturaCommandHandler(IServicoUsuario servicoUsuario, IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IServicoNotificacao servicoNotificacao)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public async Task<bool> Handle(ExecutaNotificacaoCadastroFechamentoReaberturaCommand request, CancellationToken cancellationToken)
        {
            var dreCodigo = request.DreCodigo;
            var fechamentoReabertura = request.FechamentoReabertura;
            var codigoRf = request.CodigoRf;
            var ueCodigo = request.UeCodigo;

            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRf);
            var notificacao = CriaNotificacaoCadastro(fechamentoReabertura, usuario.Id, ueCodigo, dreCodigo);
            await repositorioFechamentoReabertura.SalvarNotificacaoAsync(new FechamentoReaberturaNotificacao() { FechamentoReaberturaId = fechamentoReabertura.Id, NotificacaoId = notificacao.Id });

            return true;
        }

        private Notificacao CriaNotificacaoCadastro(FechamentoReabertura fechamentoReabertura, long usuarioId, string dreCodigo, string ueCodigo)
        {
            var tituloNotificacao = $"Período de reabertura - {fechamentoReabertura.TipoCalendario.Nome}";

            if (dreCodigo == null && ueCodigo == null)
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
