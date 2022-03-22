namespace SME.SGP.Infra.Dtos
{
    public class FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto
    {
        public FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto(long encaminhamentoId, string turmaCodigo,
            string alunoCodigo, int anoLetivo)
        {
            EncaminhamentoId = encaminhamentoId;
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
            AnoLetivo = anoLetivo;
        }

        public long EncaminhamentoId { get; }
        public string TurmaCodigo { get; }
        public string AlunoCodigo { get; }
        public int AnoLetivo { get; }
    }
}
