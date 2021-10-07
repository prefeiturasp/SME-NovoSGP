using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConselhoClasseNotaCommand : IRequest
    {
        public ExcluirConselhoClasseNotaCommand(long conselhoClasseNotaId)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
        }

        public long ConselhoClasseNotaId { get; }
    }
}

