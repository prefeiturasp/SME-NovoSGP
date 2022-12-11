namespace SME.SGP.Infra
{
    public class NotaPosConselhoDto
    {
        public long? Id { get; set; }
        public double? Nota { get; set; }
        public string NotaConceito { get; set; }
        public bool? PodeEditar { get; set; }
        public bool EmAprovacao { get; set; }
    }
}