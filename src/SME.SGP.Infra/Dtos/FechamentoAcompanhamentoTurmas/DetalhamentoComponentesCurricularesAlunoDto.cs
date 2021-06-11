using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DetalhamentoComponentesCurricularesAlunoDto
    {
        public string NomeComponenteCurricular { get; set; }
        public string NotaFechamento { get; set; }
        public string NotaPosConselho { get; set; }
        public int QuantidadeAusencia { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public double PercentualFrequencia { get; set; }

        public bool LancaNota { get; set; }
    
    }
}
