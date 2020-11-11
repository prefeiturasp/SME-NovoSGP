using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarDocumentoUseCase : AbstractUseCase, ISalvarDocumentoUseCase
    {
        public SalvarDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(SalvarDocumentoDto param)
        {
            return await mediator.Send(new SalvarDocumentoCommand(param));
        }
    }
}
