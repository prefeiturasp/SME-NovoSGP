using MediatR;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoNotaFechamentoAgrupadaUseCase : AbstractUseCase, INotificarAlteracaoNotaFechamentoAgrupadaUseCase
    {
        public NotificarAlteracaoNotaFechamentoAgrupadaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var wfSemAprovacao = await mediator.Send(ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery.Instance);
            var agruparEmTurmaBimestre = wfSemAprovacao.GroupBy(w => new { w.TurmaId, w.Bimestre });

            foreach (var turmas in agruparEmTurmaBimestre)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaNotificacaoAprovacaoFechamentoPorTurma, turmas, Guid.NewGuid(), null));

            return true;
        }
    }
}
