using System;

namespace SME.SGP.Dominio
{
    public class ConsolidacaoProdutividadeFrequencia : EntidadeBase
    {
        public string CodigoTurma { get; set; }
        public string DescricaoTurma { get; set; }
        public string CodigoUe { get; set; }
        public string DescricaoUe { get; set; }
        public string CodigoDre { get; set; }
        public string DescricaoDre { get; set; }
        public string NomeProfessor { get; set; }
        public string RfProfessor { get; set; }
        public int Bimestre { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataAula { get; set; }
        public DateTime DataRegistroFrequencia { get; set; }
        public int DiferenciaDiasDataAulaRegistroFrequencia { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoComponenteCurricular { get; set; }
        public string NomeComponenteCurricular { get; set; }
    }
}
