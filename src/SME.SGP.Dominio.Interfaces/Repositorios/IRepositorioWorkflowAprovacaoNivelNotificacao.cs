﻿using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWorkflowAprovacaoNivelNotificacao
    {
        void Salvar(WorkflowAprovacaoNivelNotificacao workflowAprovaNivelNotificacao);
        Task SalvarAsync(WorkflowAprovacaoNivelNotificacao workflowAprovaNivelNotificacao);

        Task ExcluirPorWorkflowNivelNotificacaoId(long workflowNivelId, long notificacaoId);
    }
}