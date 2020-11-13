using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ValidaSeExistePlanoAulaPorIdQuery : IRequest<bool>
    {
        public ValidaSeExistePlanoAulaPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
