namespace SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno
{
    public class AnotacaoAlunoAulaPorPeriodoDto
    {
        public string CodigoAluno { get; set; }
        public long AulaId { get; set; }
        public long? MotivoAusenciaId { get; set; }
        public string Anotacao { get; set; }
        public string DataAula { get; set; }
        public string DescricaoMotivoAusencia { get; set; }
    }
}
