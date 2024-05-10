using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasWorkflowAprovacao : IConsultasWorkflowAprovacao
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public ConsultasWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public async Task<List<WorkflowAprovacaoTimeRespostaDto>> ObtemTimelinePorCodigoNotificacao(long notificacaoId)
        {
            var workflow = await repositorioWorkflowAprovacao.ObterEntidadeCompleta(0, notificacaoId);

            if (workflow.EhNulo())
                throw new NegocioException($"Não foi possível obter o workflow através da mensagem de id {notificacaoId}");

            var listaWorkflows = new List<WorkflowAprovacaoTimeRespostaDto>();

            foreach (var nivel in workflow.ObtemNiveisUnicosEStatus())
            {
                if (nivel.Status == WorkflowAprovacaoNivelStatus.AguardandoAprovacao || nivel.Status == WorkflowAprovacaoNivelStatus.SemStatus)
                    listaWorkflows.Add(RetornaProximoUsuarioTimeline(nivel));
                else
                {
                    listaWorkflows.Add(new WorkflowAprovacaoTimeRespostaDto()
                    {
                        AlteracaoData = nivel.AlteradoEm.HasValue ? nivel.AlteradoEm.Value.ToString() : null,
                        AlteracaoUsuario = nivel.AlteradoPor,
                        AlteracaoUsuarioRf = nivel.AlteradoRF,
                        NivelDescricao = nivel.Cargo.HasValue ? nivel.Cargo.GetAttribute<DisplayAttribute>().Name : null,
                        NivelId = nivel.Id,
                        Status = nivel.Status.GetAttribute<DisplayAttribute>().Name,
                        StatusId = (int)nivel.Status,
                        Nivel = nivel.Nivel
                    });
                }
            }
            return listaWorkflows;
        }

        private WorkflowAprovacaoTimeRespostaDto RetornaProximoUsuarioTimeline(WorkflowAprovacaoNivel nivel)
        {
            if (nivel.Usuarios.Count() > 1)
                return new WorkflowAprovacaoTimeRespostaDto()
                {
                    AlteracaoData = null,
                    AlteracaoUsuario = null,
                    AlteracaoUsuarioRf = null,
                    NivelDescricao = nivel.Cargo.HasValue ? nivel.Cargo.GetAttribute<DisplayAttribute>().Name : null,
                    NivelId = nivel.Id,
                    Status = nivel.Status.GetAttribute<DisplayAttribute>().Name,
                    StatusId = (int)nivel.Status,
                    Nivel = nivel.Nivel
                };

            return new WorkflowAprovacaoTimeRespostaDto()
            {
                AlteracaoData = null,
                AlteracaoUsuario = nivel.Usuarios.Any() ? nivel.Usuarios.FirstOrDefault().Nome : "",
                AlteracaoUsuarioRf = nivel.Usuarios.Any() ? nivel.Usuarios.FirstOrDefault().CodigoRf : "",
                NivelDescricao = nivel.Cargo.HasValue ? nivel.Cargo.GetAttribute<DisplayAttribute>().Name : null,
                NivelId = nivel.Id,
                Status = nivel.Status.GetAttribute<DisplayAttribute>().Name,
                StatusId = (int)nivel.Status,
                Nivel = nivel.Nivel
            };
        }
    }
}