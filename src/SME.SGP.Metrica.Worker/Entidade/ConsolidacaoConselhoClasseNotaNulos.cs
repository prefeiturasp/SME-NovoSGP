﻿namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConsolidacaoConselhoClasseNotaNulos
    {
        public long Id { get; set; }
        public string TurmaCodigo { get; set; }
        public int Bimestre { get; set; }
        public string AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}
