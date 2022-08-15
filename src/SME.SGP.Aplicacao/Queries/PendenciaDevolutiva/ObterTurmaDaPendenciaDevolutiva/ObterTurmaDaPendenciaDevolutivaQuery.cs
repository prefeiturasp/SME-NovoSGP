using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaDevolutivaQuery : IRequest<Turma>
    {
        public ObterTurmaDaPendenciaDevolutivaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }
}
