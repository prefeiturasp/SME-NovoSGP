using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualPorIdQuery : IRequest<RegistroIndividualDto>
    {
        public ObterRegistroIndividualPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
