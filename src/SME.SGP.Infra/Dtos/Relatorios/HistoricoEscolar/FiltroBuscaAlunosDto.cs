namespace SME.SGP.Infra
{
    public class FiltroBuscaEstudanteDto
    {
        public string AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public long CodigoTurma { get; set; }
        public long? Codigo { get; set; }
        public string Nome { get; set; }
        public bool Historico { get; set; } = false;
    }
}
