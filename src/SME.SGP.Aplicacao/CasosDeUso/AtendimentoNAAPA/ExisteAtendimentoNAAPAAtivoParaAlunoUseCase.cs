using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteAtendimentoNAAPAAtivoParaAlunoUseCase : AbstractUseCase, IExisteAtendimentoNAAPAAtivoParaAlunoUseCase
    {
        public ExisteAtendimentoNAAPAAtivoParaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(string param)
        {
            return mediator.Send(new ExisteAtendimentoNAAPAAtivoParaAlunoQuery(param));
        }
    }
}
