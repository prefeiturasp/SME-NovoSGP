using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoCadastroFechamentoReaberturaCommand : IRequest<bool>
    {
        public ExecutaNotificacaoCadastroFechamentoReaberturaCommand(FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura)
        {
            FechamentoReabertura = fechamentoReabertura;
        }

        public FiltroFechamentoReaberturaNotificacaoDto FechamentoReabertura { get; set; }
    }
}
