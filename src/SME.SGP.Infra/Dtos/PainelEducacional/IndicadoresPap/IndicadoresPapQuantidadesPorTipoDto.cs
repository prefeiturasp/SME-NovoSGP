using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap
{
    public class IndicadoresPapQuantidadesPorTipoDto
    {
        public TipoPap TipoPap { get; set; }
        public string TipoPapNome => TipoPap.Name();
        public int TotalTurmas { get; set; }
        public int TotalAlunos { get; set; }
        public int TotalAlunosComFrequenciaInferiorLimite { get; set; }
        public int TotalAlunosDificuldadeTop1 { get; set; }
        public int TotalAlunosDificuldadeTop2 { get; set; }
        public int TotalAlunosDificuldadeOutras { get; set; }
    }
}
