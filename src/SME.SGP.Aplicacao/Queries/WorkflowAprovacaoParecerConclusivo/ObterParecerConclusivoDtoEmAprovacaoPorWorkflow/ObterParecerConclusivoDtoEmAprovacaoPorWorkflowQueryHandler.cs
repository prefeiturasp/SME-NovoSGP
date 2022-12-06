using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQueryHandler : IRequestHandler<ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery, IEnumerable<WFAprovacaoParecerConclusivoDto>>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;

        public ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQueryHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
        }

        public Task<IEnumerable<WFAprovacaoParecerConclusivoDto>> Handle(ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery request, CancellationToken cancellationToken)
        {
            return repositorioWFAprovacaoParecerConclusivo.ObterAprovacaoParecerConclusivoPorWorkflowId(request.WorkflowId);
        }
    }
}
