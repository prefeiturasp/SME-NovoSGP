using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaUseCase : AbstractUseCase, IExcluirOcorrenciaUseCase
    {
        public ExcluirOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(IEnumerable<long> lstIds)
        {
            var retorno = await mediator.Send(new ExcluirOcorrenciaCommand(lstIds));
            return retorno;
        }
    }
}
