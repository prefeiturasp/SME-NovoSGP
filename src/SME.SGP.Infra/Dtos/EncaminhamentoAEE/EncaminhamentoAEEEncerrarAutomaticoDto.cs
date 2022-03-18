namespace SME.SGP.Infra.Dtos
{
    public class EncaminhamentoAEEEncerrarAutomaticoDto
    {
        public long EncaminhamentoId { get; set; }
        public string AlunoCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
        public long PendenciaId { get; set;  }
    }
}
