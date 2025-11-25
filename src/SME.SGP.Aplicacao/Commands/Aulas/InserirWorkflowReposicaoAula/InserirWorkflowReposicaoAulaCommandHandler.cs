using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirWorkflowReposicaoAulaCommandHandler : IRequestHandler<InserirWorkflowReposicaoAulaCommand, long>
    {
        private readonly IConfiguration configuration;
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;

        public InserirWorkflowReposicaoAulaCommandHandler(IConfiguration configuration, IComandosWorkflowAprovacao comandosWorkflowAprovacao)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        public async Task<long> Handle(InserirWorkflowReposicaoAulaCommand request, CancellationToken cancellationToken)
        {
            var linkParaReposicaoAula = ObterLinkParaReposicaoAula(request);

            var wfAprovacaoAula = new WorkflowAprovacaoDto()
            {
                Ano = request.Ano,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = request.AulaId,
                Tipo = WorkflowAprovacaoTipo.ReposicaoAula,
                UeId = request.UeCodigo,
                DreId = request.DreCodigo,
                NotificacaoTitulo = $"Criação de Aula de Reposição na turma {request.TurmaNome}",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $"Foram criadas {request.Quantidade} aula(s) de reposição de {request.ComponenteCurricularNome} na turma {request.TurmaNome} da {request.UeNome} ({request.DreNome}). Para que esta aula seja considerada válida você precisa aceitar esta notificação. Para visualizar a aula clique  <a href='{linkParaReposicaoAula}'>aqui</a>."
            };

            wfAprovacaoAula.AdicionarNivel(Cargo.Diretor);

            var wfAprovacaoId = await comandosWorkflowAprovacao.Salvar(wfAprovacaoAula);

            return wfAprovacaoId;
        }

        private string ObterLinkParaReposicaoAula(InserirWorkflowReposicaoAulaCommand request)
        {
            var link = new StringBuilder($"{configuration["UrlFrontEnd"]}calendario-escolar/calendario-professor/cadastro-aula/editar/{request.AulaId}/true");
            link.Append($"?anoLetivo={request.Ano}");
            link.Append($"&modalidade={(int)request.TurmaModalidade}");
            link.Append($"&semestre={request.TurmaSemestre}");
            link.Append($"&dre={request.DreCodigo}");
            link.Append($"&ue={request.UeCodigo}");
            link.Append($"&turma={request.TurmaCodigo}");
            link.Append($"&turmaDescricao={request.TurmaDescricao}");
            return link.ToString();
        }
    }
}