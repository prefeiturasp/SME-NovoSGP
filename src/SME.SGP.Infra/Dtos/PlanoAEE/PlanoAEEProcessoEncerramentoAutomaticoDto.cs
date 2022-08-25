namespace SME.SGP.Infra
{
    public class PlanoAEEProcessoEncerramentoAutomaticoDto
    {
        public PlanoAEEProcessoEncerramentoAutomaticoDto(bool continuarProcessoEncerradosIndevidamente = false, int paginaEncerradosIdevidamente = 1)
        {
            ContinuarProcessoEncerradosIndevidamente = continuarProcessoEncerradosIndevidamente;
            PaginaEncerradosIdevidamente = paginaEncerradosIdevidamente;
        }
        public bool ContinuarProcessoEncerradosIndevidamente { get; set; }
        public int PaginaEncerradosIdevidamente { get; set; }
    }
}
