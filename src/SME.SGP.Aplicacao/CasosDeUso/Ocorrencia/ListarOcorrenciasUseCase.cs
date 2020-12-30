using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasUseCase : AbstractUseCase, IListarOcorrenciasUseCase
    {
        public ListarOcorrenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<IEnumerable<OcorrenciaListagemDto>> Executar(FiltroOcorrenciaListagemDto param)
        {
            throw new NotImplementedException();
        }
    }
}
