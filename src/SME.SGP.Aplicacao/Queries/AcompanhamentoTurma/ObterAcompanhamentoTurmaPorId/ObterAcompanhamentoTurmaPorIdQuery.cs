using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoTurmaPorIdQuery : IRequest<AcompanhamentoTurma>
    {
        public ObterAcompanhamentoTurmaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
