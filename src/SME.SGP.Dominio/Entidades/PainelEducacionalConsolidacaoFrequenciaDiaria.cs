using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoFrequenciaDiaria
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public NivelFrequenciaEnum NivelFrequencia { get; set; }
        public string Turma { get; set; }
        public int AnoLetivo { get; set; }
        public int TotalEstudantes { get; set; }
        public int TotalPresentes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public DateTime DataAula { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    }
}
