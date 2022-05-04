namespace SME.SGP.Infra
{
    public class RegistroFrequenciaTurmaEvasaoPorTurmaEMesDto
    {
        public long TurmaId { get; set; }
        public int QuantidadeTotalAbaixo50Porcento { get; set; }
        public int QuantidadeTotal0Porcento { get; set; }
    }
}
