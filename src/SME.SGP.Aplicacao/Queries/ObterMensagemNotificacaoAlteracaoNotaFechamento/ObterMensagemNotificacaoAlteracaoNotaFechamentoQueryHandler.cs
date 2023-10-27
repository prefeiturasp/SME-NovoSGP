using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterMensagemNotificacaoAlteracaoNotaFechamentoQueryHandler : NotificacaoNotaFechamentoCommandBase<ObterMensagemNotificacaoAlteracaoNotaFechamentoQuery,string>
    {
        public ObterMensagemNotificacaoAlteracaoNotaFechamentoQueryHandler(IMediator mediator) : base(mediator)
        {}

        public override async Task<string> Handle(ObterMensagemNotificacaoAlteracaoNotaFechamentoQuery request,CancellationToken cancellationToken)
        {
            var notificacao = await mediator.Send(new ObterNotificacaoPorIdQuery(request.NotificacaoId), cancellationToken);
            
            if (notificacao.EhNulo())
                return string.Empty;
            
            var mensagem = notificacao.Mensagem;
            
            var fechamentosNota =
                (await mediator.Send(
                    new ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery(request.WorkflowAprovacaoId),
                    cancellationToken)).ToList();
            
            if (!fechamentosNota.Any())
                return mensagem;
            
            var ehMensagemDinamica = mensagem.Contains(MENSAGEM_DINAMICA_TABELA_POR_ALUNO);

            if (!ehMensagemDinamica)
                return mensagem;

            await CarregarInformacoesParaNotificacao(fechamentosNota);
            mensagem = mensagem.Replace(MENSAGEM_DINAMICA_TABELA_POR_ALUNO, ObterTabelaNotas(fechamentosNota));
            return mensagem;
        }

    }
}