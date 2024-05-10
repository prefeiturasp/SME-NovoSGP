using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposDeReuniaoUseCase : AbstractUseCase, IObterTiposDeReuniaoUseCase
    {
        public ObterTiposDeReuniaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<IEnumerable<TipoReuniaoDto>> Executar()
        {
            return mediator.Send(new ObterTiposReuniaoNAAPAQuery());
        }
    }
}
