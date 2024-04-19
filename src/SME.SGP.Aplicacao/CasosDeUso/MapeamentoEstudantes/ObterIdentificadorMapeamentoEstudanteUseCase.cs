using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdentificadorMapeamentoEstudanteUseCase : AbstractUseCase, IObterIdentificadorMapeamentoEstudanteUseCase
    {
        public ObterIdentificadorMapeamentoEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<long?> Executar(string codigoAluno, long turmaId, int bimestre)
        {
            return mediator.Send(new ObterIdentificadorMapeamentoEstudanteQuery(codigoAluno, turmaId, bimestre));   
        }
    }
}
