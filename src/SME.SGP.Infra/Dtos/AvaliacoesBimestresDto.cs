using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AvaliacoesBimestresDto
    {
        public IEnumerable<AtividadeAvaliativa> Avaliacoes { get; set; }
        public PeriodoEscolar PeriodoAtual { get; set; }
        public int QuantidadeBimestres { get; set; }
    }
}