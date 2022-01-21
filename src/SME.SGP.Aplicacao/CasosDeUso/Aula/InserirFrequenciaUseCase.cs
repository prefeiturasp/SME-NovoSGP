using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciaUseCase : AbstractUseCase, IInserirFrequenciaUseCase
    {
        public InserirFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(FrequenciaDto param)
        {
            return await mediator.Send(new InserirFrequenciasAulaCommand(param));
        }
    }
}
