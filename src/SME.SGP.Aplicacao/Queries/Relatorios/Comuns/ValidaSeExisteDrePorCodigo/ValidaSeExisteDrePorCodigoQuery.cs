using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExisteDrePorCodigoQuery : IRequest<bool>
    {
        public ValidaSeExisteDrePorCodigoQuery(string codigoDre)
        {
            CodigoDre = codigoDre;
        }
        public string CodigoDre { get; set; }
    }
}
