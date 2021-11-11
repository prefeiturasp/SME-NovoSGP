using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
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
            int.TryParse(mensagemRabbit.Mensagem.ToString(), out int ano);

            if (!string.IsNullOrEmpty(mensagemRabbit.Mensagem.ToString()) && ano == DateTime.Now.Year)
            {
                await mediator.Send(new CalcularFrequenciaGeralCommand(ano));
                return true;
            }

            return false;
        }
    }
}
