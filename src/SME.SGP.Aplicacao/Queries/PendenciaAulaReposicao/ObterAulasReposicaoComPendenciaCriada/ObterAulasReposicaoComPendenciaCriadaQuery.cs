using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasReposicaoComPendenciaCriadaQuery : IRequest<long[]>
    {
        public ObterAulasReposicaoComPendenciaCriadaQuery(long[] aulasId)
        {
            AulasId = aulasId;
        }

        public long[] AulasId { get; set; }
    }
}
