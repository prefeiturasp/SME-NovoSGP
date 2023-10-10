using SME.SGP.Dominio;
using System.Globalization;

namespace SME.SGP.Infra
{
    public class DetalhamentoComponentesCurricularesAlunoDto
    {
        public string NomeComponenteCurricular { get; set; }
        public string NotaFechamento { get; set; }
        public string NotaPosConselho { get; set; }
        public int QuantidadeAusencia { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public string PercentualFrequencia { get; set; }
        public bool LancaNota { get; set; }

    }
}
