using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosWorkflowAprovacao : IComandosWorkflowAprovacao
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel;
        private readonly IRepositorioWorkflowAprovacaoNivelUsuario repositorioWorkflowAprovacaoNivelUsuario;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoWorkflowAprovacao servicoWorkflowAprovacao;
        private readonly IUnitOfWork unitOfWork;

        public ComandosWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao, IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel,
            IUnitOfWork unitOfWork, IServicoUsuario servicoUsuario, IRepositorioWorkflowAprovacaoNivelUsuario repositorioWorkflowAprovacaoNivelUsuario, IServicoWorkflowAprovacao servicoWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioWorkflowAprovacaoNivel = repositorioWorkflowAprovacaoNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivel));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioWorkflowAprovacaoNivelUsuario = repositorioWorkflowAprovacaoNivelUsuario ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelUsuario));
            this.servicoWorkflowAprovacao = servicoWorkflowAprovacao ?? throw new ArgumentNullException(nameof(servicoWorkflowAprovacao));
        }

        public async Task Aprovar(bool aprovar, long notificacaoId, string observacao)
        {
            var workflow = repositorioWorkflowAprovacao.ObterEntidadeCompleta(0, notificacaoId);
            if (workflow == null)
                throw new NegocioException($"Não foi possível localizar o fluxo de aprovação da notificação {notificacaoId}");

            unitOfWork.IniciarTransacao();

            await servicoWorkflowAprovacao.Aprovar(workflow, aprovar, observacao, notificacaoId);

            unitOfWork.PersistirTransacao();
        }

        public async Task ExcluirAsync(long idWorkflowAprovacao)
        {
            await servicoWorkflowAprovacao.ExcluirWorkflowNotificacoes(idWorkflowAprovacao);
        }

        public async Task<long> Salvar(WorkflowAprovacaoDto workflowAprovacaoNiveisDto)
        {
            if (workflowAprovacaoNiveisDto.Tipo != WorkflowAprovacaoTipo.Basica && workflowAprovacaoNiveisDto.EntidadeParaAprovarId == 0)
                throw new NegocioException("Para um workflow diferente de básico, é necessário informar o Id da entidade para Aprovar.");

            WorkflowAprovacao workflowAprovacao = MapearDtoParaEntidade(workflowAprovacaoNiveisDto);

            await repositorioWorkflowAprovacao.SalvarAsync(workflowAprovacao);

            foreach (var workflowAprovacaoNivel in workflowAprovacao.Niveis)
            {
                workflowAprovacaoNivel.WorkflowId = workflowAprovacao.Id;
                await repositorioWorkflowAprovacaoNivel.SalvarAsync(workflowAprovacaoNivel);

                foreach (var usuario in workflowAprovacaoNivel.Usuarios)
                {
                    repositorioWorkflowAprovacaoNivelUsuario.Salvar(new WorkflowAprovacaoNivelUsuario()
                    {
                        UsuarioId = usuario.Id,
                        WorkflowAprovacaoNivelId = workflowAprovacaoNivel.Id
                    });
                }
            }
            servicoWorkflowAprovacao.ConfiguracaoInicial(workflowAprovacao, workflowAprovacaoNiveisDto.EntidadeParaAprovarId);

            return workflowAprovacao.Id;
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
            workflowAprovacao.Tipo = workflowAprovacaoNiveisDto.Tipo;

            foreach (var nivel in workflowAprovacaoNiveisDto.Niveis)
            {
                var workflowNivel = new WorkflowAprovacaoNivel()
                {
                    Cargo = nivel.Cargo,
                    Nivel = nivel.Nivel
                };

                if (nivel.UsuariosRf != null && nivel.UsuariosRf.Length > 0)
                {
                    foreach (var usuarioRf in nivel.UsuariosRf)
                    {
                        workflowNivel.Adicionar(servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioRf));
                    }
                }

                workflowAprovacao.Adicionar(workflowNivel);
            }
            return workflowAprovacao;
        }
    }
}