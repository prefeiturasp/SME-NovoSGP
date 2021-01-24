using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AtualizaSituacaoConselhoClasseCommand : IRequest<bool>
    {
        public AtualizaSituacaoConselhoClasseCommand(long conselhoClasseId)
        {
            ConselhoClasseId = conselhoClasseId;
        }

        public long ConselhoClasseId { get; set; }
    }
}
