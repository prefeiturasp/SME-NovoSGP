namespace SME.SGP.Infra
{
    public class FiltroBuscaEstudanteDto
    {
        public string AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public long[] CodigoTurmas { get; set; }
        public long? Codigo { get; set; }
        public string Nome { get; set; }
    }
}
