using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFechamentoReaberturaCommand : IRequest<bool>
    {
        public ExecutaNotificacaoFechamentoReaberturaCommand(FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura)
        {
            FechamentoReabertura = fechamentoReabertura;
        }

        public FiltroFechamentoReaberturaNotificacaoDto FechamentoReabertura { get; set; }
    }
}
