using System;

namespace SME.SGP.Dominio
{
    public class ConsolidacaoDashBoardFrequencia
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public string TurmaAno { get; set; }
        public int semestre { get; set; }
        public DateTime DataAula { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? Mes { get; set; }
        public int ModalidadeCodigo { get; set; }
        public int Tipo { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string DreCodigo { get; set; }
        public long UeId { get; set; }
        public string DreAbreviacao { get; set; }
        public int QuantidadePresencas { get; set; }
        public int QuantidadeRemotos { get; set; }
        public int QuantidadeAusentes { get; set; }
        public DateTime CriadoEm { get; set; }
        public int TotalAulas { get; set; }
        public int TotalFrequencias { get; set; }
    }
}
