using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConselhoClasseNaoConsolidado
    {
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long UeId { get; set; }
        public string UeCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string ComponenteCurricular { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public bool EhFechamento { get; set; }
        public bool EhConselho { get; set; }
        public string CriadoRF { get; set; }
    }
}
