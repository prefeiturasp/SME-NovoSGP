using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaUseCase : AbstractUseCase, IExcluirOcorrenciaUseCase
    {
        public ExcluirOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(IEnumerable<long> param)
        {
            throw new NotImplementedException();
        }
    }
}
