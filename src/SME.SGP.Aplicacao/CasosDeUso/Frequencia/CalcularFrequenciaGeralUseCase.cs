using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaGeralUseCase : AbstractUseCase, ICalcularFrequenciaGeralUseCase
    {
        public CalcularFrequenciaGeralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            await mediator.Send(new CalcularFrequenciaGeralCommand(2021));
            return true;
        }
    }
}
