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

            if (notificacao.EhNulo())
                return string.Empty;
            
            var mensagem = notificacao.Mensagem;

            var pareceresConclusivos =
                (await mediator.Send(
                    new ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery(request.WorkflowAprovacaoId),
                    cancellationToken)).ToList();

            var parecerConclusivo = pareceresConclusivos.FirstOrDefault();

            if (parecerConclusivo.EhNulo())
                return mensagem;

            var ehMensagemDinamica = mensagem.Contains(MENSAGEM_DINAMICA_TABELA_POR_ALUNO);

            if (!ehMensagemDinamica) 
                return mensagem;
            
            await CarregarInformacoesParaNotificacao(pareceresConclusivos);
                
            var turma = await ObterTurma(parecerConclusivo.TurmaId);

            mensagem = mensagem.Replace(MENSAGEM_DINAMICA_TABELA_POR_ALUNO, ObterTabelaPareceresAlterados(pareceresConclusivos, turma));

            return mensagem;
        }
    }
}