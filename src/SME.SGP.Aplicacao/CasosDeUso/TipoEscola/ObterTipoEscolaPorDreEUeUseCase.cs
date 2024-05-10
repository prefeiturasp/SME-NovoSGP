using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorDreEUeUseCase : AbstractUseCase, IObterTipoEscolaPorDreEUeUseCase
    {
        public ObterTipoEscolaPorDreEUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TipoEscolaDto>> Executar(string dreCodigo, string ueCodigo, int[] modalidades)
            => await mediator.Send(new ObterTipoEscolaPorDreEUeQuery(dreCodigo, ueCodigo, modalidades));
    }
}
