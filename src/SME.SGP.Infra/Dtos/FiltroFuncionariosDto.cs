namespace SME.SGP.Infra
{
    public class FiltroFuncionarioDto
    {
        public string CodigoRF { get; set; }
        public string CodigoDRE { get; set; }
        public string CodigoUE { get; set; }
        public string NomeServidor { get; set; }

        public void AtualizaCodigoDre(string codigoDre)
        {
            CodigoDRE = codigoDre;
        }

        public void AtualizaCodigoUe(string codigoUe)
        {
            CodigoUE = codigoUe;
        }
    }
    
}