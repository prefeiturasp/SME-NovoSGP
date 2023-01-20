using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LancarFrequenciaAulaUseCase : AbstractUseCase, ILancarFrequenciaAulaUseCase
    {
        public LancarFrequenciaAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var frequencias = param.ObterObjetoMensagem<FrequenciaDto>();
            
            await mediator.Send(new InserirFrequenciasAulaCommand(frequencias));
            
            return true;
        }
    }
}
