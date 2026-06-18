namespace SME.SGP.Infra
{
    public class AlterarAnotacaoFrequenciaAlunoDto
    {
        public long Id { get; set; }
        public long? MotivoAusenciaId { get; set; }
        public string Anotacao { get; set; }
    }
}
