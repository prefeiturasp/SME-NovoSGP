using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConsolidacaoConselhoClasseNotaNulos
    {
        public ConsolidacaoConselhoClasseNotaNulos()
        {
            Data = DateTime.Now.Date.ToUniversalTime();
        }
        public long Id { get; set; }
        public string TurmaCodigo { get; set; }
        public int Bimestre { get; set; }
        public string AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime Data { get; set; }
    }
}
