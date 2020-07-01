namespace SME.SGP.Infra
{
    public class BuscaFuncionariosFiltroDto
    {
        public string CodigoRF { get; set; }
        public string CodigoUE { get; set; }
        public string NomeServidor { get; set; }

        public void AtualizaCodigoUe(string codigoUe)
        {
            CodigoUE = codigoUe;
        }
    }
    
}