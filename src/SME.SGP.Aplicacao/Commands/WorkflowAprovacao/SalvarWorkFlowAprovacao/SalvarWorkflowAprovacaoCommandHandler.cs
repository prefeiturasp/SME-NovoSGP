using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoCommandHandler : IRequestHandler<SalvarWorkflowAprovacaoCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel;
        private readonly IRepositorioWorkflowAprovacaoNivelUsuario repositorioWorkflowAprovacaoNivelUsuario;
        private readonly IServicoWorkflowAprovacao servicoWorkflowAprovacao;

        public SalvarWorkflowAprovacaoCommandHandler(IMediator mediator,
                                                     IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
                                                     IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel,
                                                     IRepositorioWorkflowAprovacaoNivelUsuario repositorioWorkflowAprovacaoNivelUsuario,
                                                     IServicoWorkflowAprovacao servicoWorkflowAprovacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioWorkflowAprovacaoNivel = repositorioWorkflowAprovacaoNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivel));
            this.repositorioWorkflowAprovacaoNivelUsuario = repositorioWorkflowAprovacaoNivelUsuario ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelUsuario));
            this.servicoWorkflowAprovacao = servicoWorkflowAprovacao ?? throw new ArgumentNullException(nameof(servicoWorkflowAprovacao));
        }

        public async Task<long> Handle(SalvarWorkflowAprovacaoCommand request, CancellationToken cancellationToken)
        {
            if (request.WorkflowAprovacao.Tipo != WorkflowAprovacaoTipo.Basica && request.WorkflowAprovacao.EntidadeParaAprovarId == 0)
                throw new NegocioException("Para um workflow diferente de básico, é necessário informar o Id da entidade para Aprovar.");

            WorkflowAprovacao workflowAprovacao = await MapearDtoParaEntidade(request.WorkflowAprovacao);

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
            servicoWorkflowAprovacao.ConfiguracaoInicial(workflowAprovacao, request.WorkflowAprovacao.EntidadeParaAprovarId);

            return workflowAprovacao.Id;
        }

        private async Task<WorkflowAprovacao> MapearDtoParaEntidade(WorkflowAprovacaoDto workflowAprovacaoNiveisDto)
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
                        workflowNivel.Adicionar(await ObterUsuarioPorRf(usuarioRf));
                    }
                }

                workflowAprovacao.Adicionar(workflowNivel);
            }
            return workflowAprovacao;
        }

        private async Task<Usuario> ObterUsuarioPorRf(string usuarioRf)
            => await mediator.Send(new ObterUsuarioPorRfOuCriaQuery(usuarioRf));
    }
}
