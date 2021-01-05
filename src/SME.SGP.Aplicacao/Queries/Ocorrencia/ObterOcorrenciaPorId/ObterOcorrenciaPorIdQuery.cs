using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciaPorIdQuery : IRequest<OcorrenciaDto>
    {
        public ObterOcorrenciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
