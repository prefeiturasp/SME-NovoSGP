using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap
{
    public class IndicadoresPapQuantidadesPorTipoDto
    {
        public TipoPap TipoPap { get; set; }
        public string TipoPapNome => TipoPap switch 
        { 
            TipoPap.PapColaborativo => "PAP Colaborativo",
            TipoPap.RecuperacaoAprendizagens => "Recuperação de Aprendizagens",
            TipoPap.Pap2Ano => "PAP 2º Ano",
            _ => "Desconhecido"
        };
        public int TotalTurmas { get; set; }
        public int TotalAlunos { get; set; }
        public int TotalAlunosComFrequenciaInferiorLimite { get; set; }
        public int TotalAlunosDificuldadeTop1 { get; set; }
        public int TotalAlunosDificuldadeTop2 { get; set; }
        public int TotalAlunosDificuldadeOutras { get; set; }
    }
}