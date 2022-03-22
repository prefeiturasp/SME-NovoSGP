namespace SME.SGP.Infra.Dtos
{
    public class FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto
    {
        public FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto(long encaminhamentoId, string ueCodigo,
            string alunoCodigo, int anoLetivo)
        {
            EncaminhamentoId = encaminhamentoId;
            UeCodigo = ueCodigo;
            AlunoCodigo = alunoCodigo;
            AnoLetivo = anoLetivo;
        }

        public long EncaminhamentoId { get; }
        public string UeCodigo { get; }
        public string AlunoCodigo { get; }
        public int AnoLetivo { get; }
    }
}
