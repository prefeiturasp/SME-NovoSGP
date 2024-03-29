﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasWorkflowAprovacao
    {
        Task<List<WorkflowAprovacaoTimeRespostaDto>> ObtemTimelinePorCodigoNotificacao(long notificacaoId);
    }
}