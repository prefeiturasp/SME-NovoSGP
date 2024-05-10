using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes.Rabbit
{
    public class SalvarLogViaRabbitCommandHandlerFake : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        public SalvarLogViaRabbitCommandHandlerFake()
        {
            
        }
        public async Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            var retorno = false;

            return await Task.FromResult(retorno);
        }       
    }
}
