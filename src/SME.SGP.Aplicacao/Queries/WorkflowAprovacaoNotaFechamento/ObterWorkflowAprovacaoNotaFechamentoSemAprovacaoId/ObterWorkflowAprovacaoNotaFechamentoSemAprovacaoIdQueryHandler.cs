using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.FechamentoNota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQueryHandler : IRequestHandler<ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;
        public async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> Handle(ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery request, CancellationToken cancellationToken)
          => await repositorioWfAprovacaoNotaFechamento.ObterWfAprovacaoNotaFechamentoSemWfAprovacaoId();
    }
}
