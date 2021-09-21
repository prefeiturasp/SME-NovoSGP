using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularLancaNotaUseCase : AbstractUseCase, IObterComponenteCurricularLancaNotaUseCase
    {
        public ObterComponenteCurricularLancaNotaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long componenteCurricularId)
        {
            return await mediator.Send(new ObterComponenteLancaNotaQuery(componenteCurricularId));
        }
    }
}
