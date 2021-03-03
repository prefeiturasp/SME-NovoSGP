using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosPorModalidadeECodigoUeUseCase : IObterCiclosPorModalidadeECodigoUeUseCase
    {
        private readonly IMediator mediator;

        public ObterCiclosPorModalidadeECodigoUeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        
        public async Task<IEnumerable<RetornoCicloDto>> Executar(FiltroCicloPorModalidadeECodigoUeDto filtro)
        {
            return await mediator.Send(new ObterCiclosPorModalidadeECodigoUeQuery(filtro.Modalidade, filtro.CodigoUe, filtro.ConsideraAbrangencia));
        }
    }
}
