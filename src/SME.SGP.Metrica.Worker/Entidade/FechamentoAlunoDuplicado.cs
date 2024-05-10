using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class FechamentoAlunoDuplicado
    {
        public long FechamentoTurmaDisciplinaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long UltimoId { get; set; }
    }
}
