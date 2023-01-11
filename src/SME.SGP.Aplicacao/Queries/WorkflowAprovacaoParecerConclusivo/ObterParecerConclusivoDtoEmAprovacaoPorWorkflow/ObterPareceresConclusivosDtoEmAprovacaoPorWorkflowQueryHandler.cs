using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQueryHandler : IRequestHandler<ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery, IEnumerable<WFAprovacaoParecerConclusivoDto>>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;

        public ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQueryHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
        }

        public Task<IEnumerable<WFAprovacaoParecerConclusivoDto>> Handle(ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery request, CancellationToken cancellationToken)
        {
            return repositorioWFAprovacaoParecerConclusivo.ObterAprovacaoPareceresConclusivosPorWorkflowId(request.WorkflowId);
        }
    }
}
