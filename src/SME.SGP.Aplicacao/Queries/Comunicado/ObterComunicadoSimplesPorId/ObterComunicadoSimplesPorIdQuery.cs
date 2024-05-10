using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoSimplesPorIdQuery : IRequest<Comunicado>
    {
        public ObterComunicadoSimplesPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
