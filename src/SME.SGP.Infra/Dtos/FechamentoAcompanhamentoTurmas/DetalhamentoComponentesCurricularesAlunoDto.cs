using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DetalhamentoComponentesCurricularesAlunoDto
    {
        public string NomeComponenteCurricular { get; set; }
        public float NotaFechamento { get; set; }
        public float NotaPosConselho { get; set; }
        public int QuantidadeAusencia { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public float PercentualFrequencia { get; set; }
    
    }
}
