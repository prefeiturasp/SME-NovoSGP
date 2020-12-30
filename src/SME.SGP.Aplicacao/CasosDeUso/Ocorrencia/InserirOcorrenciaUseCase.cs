using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaUseCase : AbstractUseCase, IInserirOcorrenciaUseCase
    {
        public InserirOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<AuditoriaDto> Executar(InserirOcorrenciaDto param)
        {
            throw new NotImplementedException();
        }
    }
}
