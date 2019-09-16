using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasWorkflowAprovacao
    {
        WorkflowAprovacao ObtemPorId(long id);

        IEnumerable<WorkflowAprovacaoTimeRespostaDto> ObtemTimelinePorCodigoNotificacao(long notificacaoId);
    }
}