using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviaNotificacaoCriadorCommandHandler : IRequestHandler<EnviaNotificacaoCriadorCommand, bool>
    {
        private readonly IServicoNotificacao servicoNotificacao;

        public EnviaNotificacaoCriadorCommandHandler(IServicoNotificacao servicoNotificacao)
        {
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));            
        }

        public Task<bool> Handle(EnviaNotificacaoCriadorCommand request, CancellationToken cancellationToken)
        {
            var extensaoRelatorio = request.RelatorioCorrelacao.Formato.Name();
            var urlNotificacao = $"{request.UrlRedirecionamentoBase}api/v1/downloads/sgp/{extensaoRelatorio}/{request.RelatorioCorrelacao.TipoRelatorio.ShortName()}.{extensaoRelatorio}/{request.RelatorioCorrelacao.Codigo}";

            var descricaoDoRelatorio = request.RelatorioCorrelacao.TipoRelatorio.GetAttribute<DisplayAttribute>().Description;
            var mensagem = FormatarMensagem(descricaoDoRelatorio, urlNotificacao, request.MensagemUsuario);

            var notificacao = new Notificacao()
            {
                Titulo = descricaoDoRelatorio,
                Ano = request.RelatorioCorrelacao.CriadoEm.Year,
                Categoria = NotificacaoCategoria.Aviso,
                Mensagem = mensagem,
                Tipo = NotificacaoTipo.Relatorio,
                UsuarioId = request.RelatorioCorrelacao.UsuarioSolicitanteId
            };

            servicoNotificacao.Salvar(notificacao);

            return Task.FromResult(true);
        }

        private string FormatarMensagem(string descricaoDoRelatorio, string urlNotificacao, string mensagemUsuario)
            => $"O {descricaoDoRelatorio} está pronto para download. <br /> Clique <a href='{urlNotificacao}' target='_blank'>aqui</a> <br /> Observação: Este link é válido por 24 horas. " +
                (string.IsNullOrEmpty(mensagemUsuario) ? "" : "<br/>" + mensagemUsuario);
    }
}
