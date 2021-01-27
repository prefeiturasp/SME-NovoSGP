using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioDeBordoPorIdQuery : IRequest<DiarioBordoDetalhesDto>
    {
        public ObterDiarioDeBordoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
