using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
