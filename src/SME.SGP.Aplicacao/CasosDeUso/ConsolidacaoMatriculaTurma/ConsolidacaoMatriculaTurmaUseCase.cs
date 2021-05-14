using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoMatriculaTurmaUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasUseCase
    {
        public ConsolidacaoMatriculaTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }
        public Task<bool> Executar(MensagemRabbit param)
        {
            throw new System.NotImplementedException();
        }
    }
}
