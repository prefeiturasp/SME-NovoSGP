namespace SME.SGP.Infra.Dtos
{
    public class FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA
    {
        public FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA()
        {}
        public FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA(string codigoDre, string codigoUe = null)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}