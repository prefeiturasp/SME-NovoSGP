using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery : IRequest<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        public ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery()
        {}

        private static ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery _instance;
        public static ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery Instance => _instance ??= new();
    }
}
