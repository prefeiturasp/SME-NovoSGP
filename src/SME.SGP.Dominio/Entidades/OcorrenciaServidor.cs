namespace SME.SGP.Dominio
{ 
    public class OcorrenciaServidor : EntidadeBase
    {
        public string CodigoRf { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
        public long OcorrenciaId { get; set; }
    }
}
