using MediatR;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPodeCadastrarAulaPorDataQueryHandlerFake : IRequestHandler<ObterPodeCadastrarAulaPorDataQuery, PodeCadastrarAulaPorDataRetornoDto>
    {

        public ObterPodeCadastrarAulaPorDataQueryHandlerFake()
        { }

        public Task<PodeCadastrarAulaPorDataRetornoDto> Handle(ObterPodeCadastrarAulaPorDataQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new PodeCadastrarAulaPorDataRetornoDto(true));
        }
    }
}