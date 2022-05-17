namespace SME.SGP.Dominio
{
    public class PendenciaDevolutiva : EntidadeBase
    {
        public long PedenciaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long TurmaId { get; set; }
    }
}
