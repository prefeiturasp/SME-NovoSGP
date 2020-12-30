using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciaUseCase : AbstractUseCase, IObterOcorrenciaUseCase
    {
        public ObterOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<OcorrenciaDto> Executar(long param)
        {
            throw new NotImplementedException();
        }
    }
}
