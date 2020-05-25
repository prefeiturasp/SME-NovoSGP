using MediatR;
using SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaVersaoUseCase
    {
        public static async Task<string> Executar(IMediator mediator)
        {
            return await mediator.Send(new ObterUltimaVersaoQuery());
        }
    }
}
