namespace SME.SGP.Dominio
{
    public class OcorrenciaAluno : EntidadeBase
    {
        public long CodigoAluno { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }
    }
}