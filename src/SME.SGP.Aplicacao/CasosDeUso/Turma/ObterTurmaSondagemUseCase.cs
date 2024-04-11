using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaSondagemUseCase : AbstractUseCase, IObterTurmaSondagemUseCase
    {
        public ObterTurmaSondagemUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<IEnumerable<TurmaRetornoDto>> Executar(string ueCodigo, int anoLetivo)
        {
            return mediator.Send(new ObterTurmaSondagemQuery(ueCodigo, anoLetivo));
        }
    }
}
