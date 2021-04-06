using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterDREIdPorCodigoQuery : IRequest<long>
    {
        public ObterDREIdPorCodigoQuery(string codigoDre)
        {
            CodigoDre = codigoDre;
        }

        public string CodigoDre { get; }
    }
}
