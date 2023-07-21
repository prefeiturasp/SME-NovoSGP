using System;

namespace SME.SGP.Dominio
{
    public class ConsolidacaoFrequenciaTurma
    {
        public ConsolidacaoFrequenciaTurma() { }
        public ConsolidacaoFrequenciaTurma(
                                            long turmaId, 
                                            int quantidadeAcimaMinimoFrequencia, 
                                            int quantidadeAbaixoMinimoFrequencia, 
                                            TipoConsolidadoFrequencia tipoConsolidado,
                                            DateTime? periodoInicio,
                                            DateTime? periodoFim)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
            TipoConsolidado = tipoConsolidado;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }

        public int QuantidadeAcimaMinimoFrequencia { get; set; }
        public int QuantidadeAbaixoMinimoFrequencia { get; set; }
        public TipoConsolidadoFrequencia TipoConsolidado { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
    }
}