using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConselhoClasseNotaCommand : IRequest
    {
        public ExcluirConselhoClasseNotaCommand(long conselhoClasseNotaId, bool somenteLogico = true)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
            SomenteLogico = somenteLogico;
        }

        public long ConselhoClasseNotaId { get; }
        public bool SomenteLogico { get; }
    }
}

