using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicoesResponsaveisCommand : IRequest
    {
        public IEnumerable<long> AtribuicoesIds { get; }

        public RemoverAtribuicoesResponsaveisCommand(IEnumerable<long> atribuicoesIds)
        {
            AtribuicoesIds = atribuicoesIds;
        }
    }
}