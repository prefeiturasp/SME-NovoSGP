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
            
            if (notificacao == null)
                return string.Empty;
            
            var mensagem = notificacao.Mensagem;
            
            var fechamentosNota =
                (await mediator.Send(
                    new ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery(request.WorkflowAprovacaoId),
                    cancellationToken)).ToList();
            
            if (!fechamentosNota.Any())
                return mensagem;
            
            if (notificacao.Status == NotificacaoStatus.Aceita || notificacao.Status == NotificacaoStatus.Reprovada)
                return mensagem;
            
            var contemTagMensagemDinamica = mensagem.Contains(MENSAGEM_DINAMICA_TABELA_POR_ALUNO);
            
            await CarregarInformacoesParaNotificacao(fechamentosNota);

            var complementoMensagem = ObterTabelaNotas(fechamentosNota);
            
            mensagem = contemTagMensagemDinamica 
                ? mensagem.Replace(MENSAGEM_DINAMICA_TABELA_POR_ALUNO, complementoMensagem) 
                : AtualizarMensagem(complementoMensagem, mensagem);
            
            var workflowAprovacao = await mediator.Send(new ObterWorkflowPorIdQuery(request.WorkflowAprovacaoId));
            workflowAprovacao.NotifacaoMensagem = mensagem;
            await mediator.Send(new SalvarWorkflowAprovacaoCommand(workflowAprovacao));
            
            notificacao.Mensagem = mensagem;
            await mediator.Send(new SalvarNotificacaoCommand(notificacao));

            return mensagem;
        }

        private string AtualizarMensagem(string complementoMensagem, string mensagem)
        {
            var mensagemPadrao = mensagem.Substring(0,mensagem.LastIndexOf("<table"));
            
            return $"{mensagemPadrao} {complementoMensagem}";
        }
    }
}