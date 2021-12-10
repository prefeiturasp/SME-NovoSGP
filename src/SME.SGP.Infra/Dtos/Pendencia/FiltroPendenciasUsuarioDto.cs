namespace SME.SGP.Infra
{
    public class FiltroPendenciasUsuarioDto
    {
        public FiltroPendenciasUsuarioDto(string turmaCodigo, int? tipoPendencia, string tituloPendencia)
        {
            TurmaCodigo = turmaCodigo;
            TipoPendencia = tipoPendencia;
            TituloPendencia = tituloPendencia;
        }

        public string TurmaCodigo { get; set; }

        public int? TipoPendencia { get; set; }

        public string TituloPendencia { get; set; }
    }
}
