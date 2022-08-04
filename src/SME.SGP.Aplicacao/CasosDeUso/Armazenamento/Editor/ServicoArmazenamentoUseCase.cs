using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ServicoArmazenamentoUseCase: AbstractUseCase, IServicoArmazenamentoUseCase
    {
        public ServicoArmazenamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(bool ativo)
        {
            return await mediator.Send(new ObterBucketsServicoArmazenamentoQuery());
        }
    }
}
