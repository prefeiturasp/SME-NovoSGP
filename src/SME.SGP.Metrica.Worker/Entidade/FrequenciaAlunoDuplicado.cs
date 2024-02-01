using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class FrequenciaAlunoDuplicado
    {
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public string ComponenteCurricularId { get; set; }
        public int Tipo { get; set; }
        public long UeId { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long PrimeiroId { get; set; }
        public long UltimoId { get; set; }
    }
}
