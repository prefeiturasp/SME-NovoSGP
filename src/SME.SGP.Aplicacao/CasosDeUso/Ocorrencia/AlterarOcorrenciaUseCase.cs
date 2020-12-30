using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaUseCase : AbstractUseCase, IAlterarOcorrenciaUseCase
    {
        public AlterarOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<AuditoriaDto> Executar(AlterarOcorrenciaDto param)
        {
            throw new NotImplementedException();
        }
    }
}
