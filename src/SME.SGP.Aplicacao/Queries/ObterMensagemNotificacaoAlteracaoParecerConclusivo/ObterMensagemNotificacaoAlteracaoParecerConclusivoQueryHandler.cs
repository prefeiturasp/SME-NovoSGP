using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterMensagemNotificacaoAlteracaoParecerConclusivoQueryHandler :
        NotificacaoParecerConclusivoConselhoClasseCommandBase<ObterMensagemNotificacaoAlteracaoParecerConclusivoQuery,
            string>
    {
        public ObterMensagemNotificacaoAlteracaoParecerConclusivoQueryHandler(IMediator mediator) : base(mediator)
        {
        }

        public override async Task<string> Handle(ObterMensagemNotificacaoAlteracaoParecerConclusivoQuery request,
            CancellationToken cancellationToken)
        {
            var notificacao =
                await mediator.Send(new ObterNotificacaoPorIdQuery(request.NotificacaoId), cancellationToken);

            if (notificacao == null)
                return string.Empty;

            if (notificacao.Status is NotificacaoStatus.Aceita or NotificacaoStatus.Reprovada)
                return notificacao.Mensagem;

            var pareceresConclusivos =
                (await mediator.Send(
                    new ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery(request.WorkflowAprovacaoId),
                    cancellationToken)).ToList();

            var parecerConclusivo = pareceresConclusivos.FirstOrDefault();

            if (parecerConclusivo == null)
                return notificacao.Mensagem;

            await CarregarInformacoesParaNotificacao(pareceresConclusivos);

            var turma = await ObterTurma(parecerConclusivo.TurmaId);
            return ObterMensagem(turma, pareceresConclusivos);
        }

        protected override string ObterTextoCabecalho(Turma turma)
        {
            return
                $"O parecer conclusivo dos estudantes abaixo da turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) de {turma.AnoLetivo} foram alterados.";
        }

        protected override string ObterTextoRodape()
        {
            return "Você precisa aceitar esta notificação para que a alteração seja considerada válida.";
        }
    }
}