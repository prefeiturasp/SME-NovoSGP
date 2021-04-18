using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterDREPorIdQuery : IRequest<Dre>
    {
        public ObterDREPorIdQuery(long dreId)
        {
            DreId = dreId;
        }

        public long DreId { get; }
    }
}
