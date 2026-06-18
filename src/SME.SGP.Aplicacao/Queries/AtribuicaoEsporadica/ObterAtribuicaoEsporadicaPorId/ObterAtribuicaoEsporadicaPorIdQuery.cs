using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicaoEsporadicaPorIdQuery : IRequest<AtribuicaoEsporadica>
    {
        public ObterAtribuicaoEsporadicaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
