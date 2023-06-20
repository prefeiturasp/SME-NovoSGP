using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EncaminhamentoAee
{
    public class AtualizaEncaminhamentoAEETurmaAlunoTratarUseCase : AbstractUseCase, IAtualizaEncaminhamentoAEETurmaAlunoTratarUseCase
    {
        public AtualizaEncaminhamentoAEETurmaAlunoTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var command = param.ObterObjetoMensagem<SalvarEncaminhamentoAEETurmaAlunoCommand>();
            return await mediator.Send(command);
        }
    }
}
