namespace SME.SGP.Infra
{
    public class PeriodosParaConsultaNotasDto
    {
        public PeriodosParaConsultaNotasDto(long periodoInicioTicks, long periodoFimTicks, long periodoEscolarId, int bimestre, bool ehBimestreAtual)
        {
            PeriodoInicioTicks = periodoInicioTicks;
            PeriodoFimTicks = periodoFimTicks;
            PeriodoEscolarId = periodoEscolarId;
            Bimestre = bimestre;
            EhBimestreAtual = ehBimestreAtual;
        }

        public long PeriodoInicioTicks { get; set; }
        public long PeriodoFimTicks { get; set; }
        public long PeriodoEscolarId { get; set; }
        public int Bimestre { get; set; }
        public bool EhBimestreAtual { get; set; }
    }
}
