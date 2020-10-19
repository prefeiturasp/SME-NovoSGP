using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalUsuariosComAcessoIncompletoQuery : IRequest<long>
    {
        public string CodigoDre { get; set; }
        public long CodigoUe { get; set; }

        public ObterTotalUsuariosComAcessoIncompletoQuery(string codigoDre, long codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
    }
}
