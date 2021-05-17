using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaSyncUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmaSyncUseCase
    {
        public ConciliacaoFrequenciaTurmaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            SentrySdk.AddBreadcrumb($"Mensagem ConciliacaoFrequenciaTurmaSyncUseCase", "Rabbit - ConciliacaoFrequenciaTurmaSyncUseCase");

            var commad = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaTurmasCommand>();

            return await mediator.Send(commad);
        }
    }
}
