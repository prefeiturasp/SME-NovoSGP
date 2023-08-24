using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizaPlanoAEETurmaAlunoTratarUseCase : AbstractUseCase, IAtualizaPlanoAEETurmaAlunoTratarUseCase
    {
        public AtualizaPlanoAEETurmaAlunoTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<SalvarPlanoAEETurmaAlunoCommand>();
            return await mediator.Send(command);
        }
    }
}
