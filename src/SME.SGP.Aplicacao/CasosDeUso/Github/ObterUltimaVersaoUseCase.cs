using MediatR;
using SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease;
using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaVersaoUseCase : IObterUltimaVersaoUseCase
    {
        private readonly IMediator mediator;

        public ObterUltimaVersaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Executar()
        {
            return await mediator.Send(new ObterUltimaVersaoQuery());
        }
    }
}
