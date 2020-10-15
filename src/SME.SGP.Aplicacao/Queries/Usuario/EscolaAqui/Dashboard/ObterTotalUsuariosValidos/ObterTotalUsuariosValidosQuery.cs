using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalUsuariosValidosQuery : IRequest<long>
    {
        public string CodigoDre { get; set; }
        public long CodigoUe { get; set; }

        public ObterTotalUsuariosValidosQuery(string codigoDre, long codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
    }
}
