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
            
            if (notificacao.Status == NotificacaoStatus.Aceita || notificacao.Status == NotificacaoStatus.Reprovada || !TemTagDinamicaOuFixa(mensagem))
                return mensagem;
            
            var contemTagMensagemDinamica = mensagem.Contains(TAG_MENSAGEM_DINAMICA_TABELA_POR_ALUNO);
            
            await CarregarInformacoesParaNotificacao(fechamentosNota);

            var complementoMensagem = ObterTabelaNotas(fechamentosNota);
            
            var mensagemAlterada = contemTagMensagemDinamica 
                ? mensagem.Replace(TAG_MENSAGEM_DINAMICA_TABELA_POR_ALUNO, complementoMensagem) 
                : AtualizarMensagem(complementoMensagem, mensagem);

            if (!mensagemAlterada.Equals(mensagem))
            {
                var workflowAprovacao = await mediator.Send(new ObterWorkflowPorIdQuery(request.WorkflowAprovacaoId));
                workflowAprovacao.NotifacaoMensagem = mensagemAlterada;
                await mediator.Send(new SalvarWorkflowAprovacaoCommand(workflowAprovacao));
            
                var notificacoes = await mediator.Send(new ObterNotificacoesPorWorkFlowAprovacaoIdQuery(request.WorkflowAprovacaoId));
                await mediator.Send(new AtualizarNotificacaoMensagemPorIdsCommand(notificacoes.Select(s=> s.Id).ToList().ToArray(), mensagemAlterada));
                
                return mensagemAlterada;
            }
            return mensagem;
        }

        private bool TemTagDinamicaOuFixa(string mensagem)
        {
            return mensagem.Contains(TAG_MENSAGEM_DINAMICA_TABELA_POR_ALUNO) || mensagem.Contains(MENSAGEM_FIXA_TABELA_POR_ALUNO);
        }

        private string AtualizarMensagem(string complementoMensagem, string mensagem)
        {
            var mensagemPadrao = mensagem.Substring(0,mensagem.LastIndexOf(TAG_MENSAGEM_FIXA_POR_ALUNO));
            
            return $"{mensagemPadrao} {complementoMensagem}";
        }
    }
}