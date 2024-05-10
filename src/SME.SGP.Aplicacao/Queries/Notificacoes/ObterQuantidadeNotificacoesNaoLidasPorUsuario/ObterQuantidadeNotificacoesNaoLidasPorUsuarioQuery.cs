using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery : IRequest<int>
    {
        public ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery(int anoLetivo, string codigoRf)
        {
            AnoLetivo = anoLetivo;
            CodigoRf = codigoRf;
        }

        public int AnoLetivo { get; set; }
        public string CodigoRf { get; set; }
    }
}
