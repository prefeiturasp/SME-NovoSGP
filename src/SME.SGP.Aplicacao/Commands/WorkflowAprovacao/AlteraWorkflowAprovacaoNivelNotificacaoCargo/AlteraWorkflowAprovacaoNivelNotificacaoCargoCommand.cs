using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Commands
{
    public class AlteraWorkflowAprovacaoNivelNotificacaoCargoCommand : IRequest<bool>
    {
        public AlteraWorkflowAprovacaoNivelNotificacaoCargoCommand(long workflowId, long notificacaoId, List<FuncionarioCargoDTO> funcionariosCargos)
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
