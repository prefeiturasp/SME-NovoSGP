using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalculoFrequenciaTurmaDisciplinaUseCase : AbstractUseCase, ICalculoFrequenciaTurmaDisciplinaUseCase
    {
        public CalculoFrequenciaTurmaDisciplinaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var comando = mensagemRabbit.ObterObjetoMensagem<CalcularFrequenciaPorTurmaCommand>();
            if (comando != null)
            {
                await mediator.Send(comando);
                return true;
            }
            return false;
        }
    }
}
