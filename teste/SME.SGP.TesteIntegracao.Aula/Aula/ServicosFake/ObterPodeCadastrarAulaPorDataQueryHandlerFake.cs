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

        public async Task<PodeCadastrarAulaPorDataRetornoDto> Handle(ObterPodeCadastrarAulaPorDataQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PodeCadastrarAulaPorDataRetornoDto(true));
        }
    }
}