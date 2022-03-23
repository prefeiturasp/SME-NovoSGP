namespace SME.SGP.Infra.Dtos
{
    public class EncaminhamentoAEEVigenteDto
    {
        public long EncaminhamentoId { get; set; }
        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public string UeCodigo { get; set; }
    }
}
