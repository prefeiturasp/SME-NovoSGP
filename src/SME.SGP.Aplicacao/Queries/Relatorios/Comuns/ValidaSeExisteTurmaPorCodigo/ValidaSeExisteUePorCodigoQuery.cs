using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExisteUePorCodigoQuery : IRequest<bool>
    {
        public ValidaSeExisteUePorCodigoQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }
        public string CodigoUe { get; set; }
    }
}
