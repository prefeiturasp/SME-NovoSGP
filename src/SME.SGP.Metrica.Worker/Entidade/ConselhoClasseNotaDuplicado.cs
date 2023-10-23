using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConselhoClasseNotaDuplicado
    {
        public long ConselhoClasseAlunoId { get; set; }
        public long ComponenteCorricularId { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long UltimoId { get; set; }
    }
}
