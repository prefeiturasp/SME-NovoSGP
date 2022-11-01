namespace SME.SGP.Dominio
{ 
    public class OcorrenciaServidor : EntidadeBase
    {
        public string CodigoServidor { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }
    }
}
