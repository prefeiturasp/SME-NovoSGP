using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaEmAprovacaoPorFechamentoNotaIdQueryHandler : IRequestHandler<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery, IEnumerable<FechamentoNotaAprovacaoDto>>
    {
        public Task<IEnumerable<FechamentoNotaAprovacaoDto>> Handle(ObterNotaEmAprovacaoPorFechamentoNotaIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
