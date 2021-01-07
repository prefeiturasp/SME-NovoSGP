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

        public async Task<OcorrenciaDto> Executar(long id)
        {
            var retorno = await mediator.Send(new ObterOcorrenciaPorIdQuery(id));
            return retorno;
        }
    }
}
