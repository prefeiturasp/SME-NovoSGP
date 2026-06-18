namespace SME.SGP.Infra
{
    public class FiltroNotificacaoFechamentoReaberturaUEDto
    {
        public FiltroNotificacaoFechamentoReaberturaUEDto(FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura)
        {
            FechamentoReabertura = fechamentoReabertura;
        }

        public FiltroFechamentoReaberturaNotificacaoDto FechamentoReabertura { get; set; }
    }
}
