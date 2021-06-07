using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class ConsolidacaoDevolutivaTurmaDTO
    {
        public long TurmaId { get; set; }
        public int QuantidadeEstimadaDevolutivas { get; set; }
        public int QuantidadeRegistradaDevolutivas { get; set; }
    }
}
