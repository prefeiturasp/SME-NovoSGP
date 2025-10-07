using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoSondagemEscritaUe
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int PreSilabico { get; set; }
        public int SilabicoSemValor { get; set; }
        public int SilabicoComValor { get; set; }
        public int SilabicoAlfabetico { get; set; }
        public int Alfabetico { get; set; }
        public int SemPreenchimento { get; set; }
        public int SerieAno { get; set; }
        public int AnoLetivo { get; set; }
        public int QuantidadeAluno { get; set; }
        public int Bimestre { get; set; }
        public DateTimeOffset CriadoEm { get; private set; } = DateTimeOffset.Now;
    }
}
