using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTiposOcorrenciaUseCase : AbstractUseCase, IListarTiposOcorrenciaUseCase
    {
        public ListarTiposOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<OcorrenciaTipoDto>> Executar()
        {
            var retorno = await mediator.Send(new ListarTiposOcorrenciaQuery());
            return retorno;
        }
    }
}
