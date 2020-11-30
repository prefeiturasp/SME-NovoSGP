using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands
{
    public class AlteraWorkflowAprovacaoNotificacaoCargoCommand : IRequest<bool>
    {
        public AlteraWorkflowAprovacaoNotificacaoCargoCommand(long workflowId, long notificacaoId, List<FuncionarioCargoDTO> funcionariosCargos)
        {
            WorkflowId = workflowId;
            NotificacaoId = notificacaoId;
            FuncionariosCargos = funcionariosCargos;
        }

        public long WorkflowId { get; set; }

        public long NotificacaoId { get; set; }
        public List<FuncionarioCargoDTO> FuncionariosCargos { get; set; }
    }
}
