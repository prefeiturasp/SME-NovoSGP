using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase : AbstractUseCase, IExisteAtendimentoNAAPAAtivoParaAlunoUseCase
    {
        public ExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(string param)
        {
            return mediator.Send(new ExisteEncaminhamentoNAAPAAtivoParaAlunoQuery(param));
        }
    }
}
