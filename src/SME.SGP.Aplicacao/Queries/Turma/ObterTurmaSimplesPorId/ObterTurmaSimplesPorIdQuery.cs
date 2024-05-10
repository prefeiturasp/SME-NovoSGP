using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaSimplesPorIdQuery : IRequest<ObterTurmaSimplesPorIdRetornoDto>
    {
        public ObterTurmaSimplesPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
