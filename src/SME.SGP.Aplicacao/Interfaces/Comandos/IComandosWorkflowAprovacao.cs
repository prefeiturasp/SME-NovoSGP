﻿using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosWorkflowAprovacao
    {
        void Aprovar(bool aprovar, long notificacaoId, string observacao);

        Task ExcluirAsync(long idWorkflowAprovacao);

        long Salvar(WorkflowAprovacaoDto workflowAprovacaoNiveisDto);
    }
}