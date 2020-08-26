using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class EnviaNotificacaoCriadorCommand : IRequest<bool>
    {
        public EnviaNotificacaoCriadorCommand(RelatorioCorrelacao relatorioCorrelacao, string urlRedirecionamentoBase, string mensagemUsuario = "")
        {
            RelatorioCorrelacao = relatorioCorrelacao;
            UrlRedirecionamentoBase = urlRedirecionamentoBase;
            MensagemUsuario = mensagemUsuario;
        }

        public RelatorioCorrelacao RelatorioCorrelacao { get; set; }
        public string UrlRedirecionamentoBase { get; set; }
        public string MensagemUsuario { get; set; }

    }
}
