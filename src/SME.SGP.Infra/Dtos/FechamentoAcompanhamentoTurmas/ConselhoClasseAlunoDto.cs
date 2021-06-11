using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoDto
    {
        public int NumeroChamada { get; set; }
        public string AlunoCodigo { get; set; }
        public string NomeAluno { get; set; }
        public string SituacaoFechamento { get; set; }
        public int SituacaoFechamentoCodigo { get; set; }
        public double FrequenciaGlobal { get; set; }
        public bool PodeExpandir { get; set; }
        public string ParecerConclusivo { get; set; }
    }
}
