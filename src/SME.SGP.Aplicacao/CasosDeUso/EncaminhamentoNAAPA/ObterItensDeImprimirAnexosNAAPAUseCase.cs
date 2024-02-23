using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItensDeImprimirAnexosNAAPAUseCase : AbstractUseCase, IObterItensDeImprimirAnexosNAAPAUseCase
    {
        public ObterItensDeImprimirAnexosNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<IEnumerable<ImprimirAnexoDto>> Executar(long param)
        {
            return mediator.Send(new ObterItensDeImprimirAnexosNAAPAQuery(param));
        }
    }
}
