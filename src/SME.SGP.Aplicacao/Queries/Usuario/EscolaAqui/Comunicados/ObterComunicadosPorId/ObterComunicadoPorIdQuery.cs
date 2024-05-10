using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoPorIdQuery : IRequest<ComunicadoCompletoDto>
    {
        public long Id { get; set; }

        public ObterComunicadoPorIdQuery(long id)
        {
            Id = id;
        }
    }
}
