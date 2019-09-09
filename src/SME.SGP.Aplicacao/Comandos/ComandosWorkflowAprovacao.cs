using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosWorkflowAprovacao : IComandosWorkflowAprovacao
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel;
        private readonly IUnitOfWork unitOfWork;

        public ComandosWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao, IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel,
            IUnitOfWork unitOfWork)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioWorkflowAprovacaoNivel = repositorioWorkflowAprovacaoNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivel));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Salvar(WorkflowAprovacaoNiveisDto workflowAprovacaoNiveisDto)
        {
            WorkflowAprovacao workflowAprovacao = MapearDtoParaEntidade(workflowAprovacaoNiveisDto);

            unitOfWork.IniciarTransacao();

            repositorioWorkflowAprovacao.Salvar(workflowAprovacao);

            foreach (var workflowAprovacaoNivel in workflowAprovacao.Niveis)
            {
                repositorioWorkflowAprovacaoNivel.Salvar(workflowAprovacaoNivel);
            }

            unitOfWork.PersistirTransacao();
        }

        private WorkflowAprovacao MapearDtoParaEntidade(WorkflowAprovacaoNiveisDto workflowAprovacaoNiveisDto)
        {
            WorkflowAprovacao workflowAprovacao = new WorkflowAprovacao();
            workflowAprovacao.Ano = workflowAprovacaoNiveisDto.Ano;
            workflowAprovacao.DreId = workflowAprovacaoNiveisDto.DreId;
            workflowAprovacao.EscolaId = workflowAprovacaoNiveisDto.EscolaId;
            workflowAprovacao.TurmaId = workflowAprovacaoNiveisDto.TurmaId;
            workflowAprovacao.NotifacaoMensagem = workflowAprovacaoNiveisDto.NotificacaoMensagem;
            workflowAprovacao.NotifacaoTitulo = workflowAprovacaoNiveisDto.NotificacaoTitulo;
            workflowAprovacao.NotificacaoTipo = workflowAprovacaoNiveisDto.NotificacaoTipo;

            foreach (var nivel in workflowAprovacaoNiveisDto.Niveis)
            {
                workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel()
                {
                    Descricao = nivel.Descricao,
                    Nivel = nivel.Nivel,
                    UsuarioId = nivel.UsuarioId
                });
            }
            return workflowAprovacao;
        }
    }
}