using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
            var linkParaReposicaoAula = $"{configuration["UrlFrontEnd"]}calendario-escolar/calendario-professor/cadastro-aula/editar/:{request.AulaId}/";

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

            wfAprovacaoAula.AdicionarNivel(Cargo.CP);
            wfAprovacaoAula.AdicionarNivel(Cargo.Diretor);

            return await comandosWorkflowAprovacao.Salvar(wfAprovacaoAula);
        }
    }
}
