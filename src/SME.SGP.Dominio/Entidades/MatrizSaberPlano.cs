namespace SME.SGP.Dominio
{
    public class MatrizSaberPlano : EntidadeBase
    {
        public long PlanoId { get; set; }
        public long MatrizSaberId { get; set; }
        public MatrizSaber MatrizSaber { get; set; }
    }
}
