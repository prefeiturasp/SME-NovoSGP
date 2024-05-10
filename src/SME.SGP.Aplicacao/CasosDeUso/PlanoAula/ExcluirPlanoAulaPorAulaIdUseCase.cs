using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAulaPorAulaIdUseCase : AbstractUseCase, IExcluirPlanoAulaPorAulaIdUseCase
    {
        public ExcluirPlanoAulaPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            await mediator.Send(new ExcluirPlanoAulaDaAulaCommand(filtro.Id));
            return true;
        }
    }
}
