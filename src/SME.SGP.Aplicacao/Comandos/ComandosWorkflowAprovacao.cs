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
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao, IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel,
            IUnitOfWork unitOfWork, IServicoUsuario servicoUsuario)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioWorkflowAprovacaoNivel = repositorioWorkflowAprovacaoNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivel));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public void Salvar(WorkflowAprovacaoDto workflowAprovacaoNiveisDto)
        {
            WorkflowAprovacao workflowAprovacao = MapearDtoParaEntidade(workflowAprovacaoNiveisDto);

            unitOfWork.IniciarTransacao();

            repositorioWorkflowAprovacao.Salvar(workflowAprovacao);

            foreach (var workflowAprovacaoNivel in workflowAprovacao.Niveis)
            {
                workflowAprovacaoNivel.WorkflowId = workflowAprovacao.Id;
                repositorioWorkflowAprovacaoNivel.Salvar(workflowAprovacaoNivel);
            }

            unitOfWork.PersistirTransacao();
        }

        private WorkflowAprovacao MapearDtoParaEntidade(WorkflowAprovacaoDto workflowAprovacaoNiveisDto)
        {
            WorkflowAprovacao workflowAprovacao = new WorkflowAprovacao();
            workflowAprovacao.Ano = workflowAprovacaoNiveisDto.Ano;
            workflowAprovacao.DreId = workflowAprovacaoNiveisDto.DreId;
            workflowAprovacao.UeId = workflowAprovacaoNiveisDto.UeId;
            workflowAprovacao.TurmaId = workflowAprovacaoNiveisDto.TurmaId;
            workflowAprovacao.NotifacaoMensagem = workflowAprovacaoNiveisDto.NotificacaoMensagem;
            workflowAprovacao.NotifacaoTitulo = workflowAprovacaoNiveisDto.NotificacaoTitulo;
            workflowAprovacao.NotificacaoTipo = workflowAprovacaoNiveisDto.NotificacaoTipo;
            workflowAprovacao.NotificacaoCategoria = workflowAprovacaoNiveisDto.NotificacaoCategoria;

            foreach (var nivel in workflowAprovacaoNiveisDto.Niveis)
            {
                var workflowNivel = new WorkflowAprovacaoNivel()
                {
                    Cargo = nivel.Cargo,
                    Nivel = nivel.Nivel
                };

                if (nivel.UsuariosRf.Length > 0)
                {
                    foreach (var usuarioRf in nivel.UsuariosRf)
                    {
                        workflowNivel.Adicionar(servicoUsuario.ObterUsuarioPorCodigoRfOuAdiciona(usuarioRf));
                    }
                }

                workflowAprovacao.Adicionar(workflowNivel);
            }
            return workflowAprovacao;
        }
    }
}