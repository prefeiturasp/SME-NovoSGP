using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConsolidacaoFrequenciaTurma
    {
        public ConsolidacaoFrequenciaTurma() { }
        public ConsolidacaoFrequenciaTurma(
                                            long turmaId, 
                                            int quantidadeAcimaMinimoFrequencia, 
                                            int quantidadeAbaixoMinimoFrequencia, 
                                            TipoConsolidadoFrequencia tipoConsolidacao,
                                            DateTime? periodoInicio,
                                            DateTime? periodoFim)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
            TipoConsolidacao = tipoConsolidacao;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }

        public int QuantidadeAcimaMinimoFrequencia { get; set; }
        public int QuantidadeAbaixoMinimoFrequencia { get; set; }
        public TipoConsolidadoFrequencia TipoConsolidacao { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
        public int TotalAulas { get; set; }
        public int TotalFrequencias { get; set; }
    }
}