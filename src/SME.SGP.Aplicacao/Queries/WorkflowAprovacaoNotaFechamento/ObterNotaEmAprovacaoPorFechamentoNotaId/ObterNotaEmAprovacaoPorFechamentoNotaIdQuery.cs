using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaEmAprovacaoPorFechamentoNotaIdQuery : IRequest<IEnumerable<FechamentoNotaAprovacaoDto>>
    {
        public IEnumerable<long> IdsFechamentoNota { get; set; }
    }
}
