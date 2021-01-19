using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivoAusenciaPorIdQuery : IRequest<MotivoAusencia>
    {
        public ObterMotivoAusenciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
