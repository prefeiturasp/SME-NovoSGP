using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterMensagemNotificacaoAlteracaoNotaPosConselhoQueryHandler :
        AprovacaoNotaConselhoCommandBase<ObterMensagemNotificacaoAlteracaoNotaPosConselhoQuery, string>
    {
        public ObterMensagemNotificacaoAlteracaoNotaPosConselhoQueryHandler(IMediator mediator) : base(mediator)
        {
        }

        public override async Task<string> Handle(ObterMensagemNotificacaoAlteracaoNotaPosConselhoQuery request,
            CancellationToken cancellationToken)
        {
            var notificacao =
                await mediator.Send(new ObterNotificacaoPorIdQuery(request.NotificacaoId), cancellationToken);

            if (notificacao == null)
                return string.Empty;
            
            var mensagem = notificacao.Mensagem;

            var notasPosConselho =
                (await mediator.Send(
                    new ObterWfsAprovacaoNotaConselhoPorWorkflowQuery(request.WorkflowAprovacaoId),
                    cancellationToken)).ToList();

            var notaPosConselho = notasPosConselho.FirstOrDefault();

            if (notaPosConselho == null)
                return mensagem;

            var ehMensagemDinamica = mensagem.Contains(MENSAGEM_DINAMICA_TABELA_POR_ALUNO);

            if (!ehMensagemDinamica) 
                return mensagem;
            
            await CarregarInformacoesParaNotificacao(notasPosConselho);
            var turma = WFAprovacoes?.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma;
          
            mensagem = mensagem.Replace(MENSAGEM_DINAMICA_TABELA_POR_ALUNO, ObterTabelaDosAlunos(notasPosConselho, turma));

            return mensagem;
        }

    }
}