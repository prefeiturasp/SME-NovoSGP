using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class EnviaNotificacaoCriadorCommand : IRequest<bool>
    {
        public EnviaNotificacaoCriadorCommand(RelatorioCorrelacao relatorioCorrelacao, string urlRedirecionamentoBase)
        {
            RelatorioCorrelacao = relatorioCorrelacao;
            UrlRedirecionamentoBase = urlRedirecionamentoBase;
        }

        public RelatorioCorrelacao RelatorioCorrelacao { get; set; }
        public string UrlRedirecionamentoBase { get; set; }

    }
}
