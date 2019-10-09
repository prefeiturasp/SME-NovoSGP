using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Aplicacao
{
    public class ConsultasWorkflowAprovacao : IConsultasWorkflowAprovacao
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public ConsultasWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public WorkflowAprovacao ObtemPorId(long id)
        {
            var wf = repositorioWorkflowAprovacao.ObterEntidadeCompleta(0, id);
            return wf;
        }

        public List<WorkflowAprovacaoTimeRespostaDto> ObtemTimelinePorCodigoNotificacao(long notificacaoId)
        {
            var workflow = repositorioWorkflowAprovacao.ObterEntidadeCompleta(0, notificacaoId);

            if (workflow == null)
                throw new NegocioException($"Não foi possível obter o workflow através da mensagem de id {notificacaoId}");

            var listaWorkflows = new List<WorkflowAprovacaoTimeRespostaDto>();

            foreach (var nivel in workflow.ObtemNiveisUnicosEStatus())
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
            return listaWorkflows;
        }
    }
}