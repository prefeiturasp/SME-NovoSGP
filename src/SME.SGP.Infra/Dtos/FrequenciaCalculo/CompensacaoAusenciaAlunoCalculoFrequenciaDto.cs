namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoCalculoFrequenciaDto
    {
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public int Compensacoes { get; set; }
        public string ComponenteCurricularId { get; set; }
    }
}
