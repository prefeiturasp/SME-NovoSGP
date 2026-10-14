using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConselhoEmAprovacaoPorIdsQuery : IRequest<IEnumerable<ConselhoClasseNotaAprovacaoDto>>
    {
        public ObterNotasConselhoEmAprovacaoPorIdsQuery(IEnumerable<long> idsConselhoClasseNota)
        {
            IdsConselhoClasseNota = idsConselhoClasseNota;
        }

        public IEnumerable<long> IdsConselhoClasseNota { get; set; }
    }
}
