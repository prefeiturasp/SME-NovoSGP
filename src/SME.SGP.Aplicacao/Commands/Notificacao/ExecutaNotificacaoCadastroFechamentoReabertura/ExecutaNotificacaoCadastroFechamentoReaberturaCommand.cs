using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoCadastroFechamentoReaberturaCommand : IRequest<bool>
    {
        public ExecutaNotificacaoCadastroFechamentoReaberturaCommand(FechamentoReabertura fechamentoReabertura, string dreCodigo, string ueCodigo, string adminsSgpDre)
        {
            FechamentoReabertura = fechamentoReabertura;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            CodigoRf = adminsSgpDre;
        }

        public FechamentoReabertura FechamentoReabertura { get; set; }
        public string DreCodigo { get; set; }
        public string CodigoRf { get; set; }
        public string UeCodigo { get; set; }

    }
}
