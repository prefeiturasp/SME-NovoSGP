using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioDeBordoSemObservacoesPorIdQuery : IRequest<DiarioBordoListaoDto>
    {
        public ObterDiarioDeBordoSemObservacoesPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
