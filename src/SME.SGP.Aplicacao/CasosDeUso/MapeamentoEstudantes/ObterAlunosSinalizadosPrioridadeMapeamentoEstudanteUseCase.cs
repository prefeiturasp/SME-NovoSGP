using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase : AbstractUseCase, IObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase
    {
        public ObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<string[]> Executar(long turmaId)
        {
            return mediator.Send(new ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery(turmaId));   
        }
    }
}
