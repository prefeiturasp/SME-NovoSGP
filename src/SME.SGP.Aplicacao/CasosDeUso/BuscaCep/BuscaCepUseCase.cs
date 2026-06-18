using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class BuscaCepUseCase : AbstractUseCase, IBuscaCepUseCase
    {
        public BuscaCepUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<CepDto> Executar(string cep)
        {
            return await mediator.Send(new ObterCepQuery(cep));
        }
    }
}