namespace SME.SGP.Dominio.Entidades
{
    public class OcorrenciaAluno : EntidadeBase
    {
        public string CodigoAluno { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }
    }
}