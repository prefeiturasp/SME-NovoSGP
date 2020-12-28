namespace SME.SGP.Dominio.Entidades
{
    public class OcorrenciaAluno
    {
        public long Id { get; set; }
        public long CodigoAluno { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }
    }
}