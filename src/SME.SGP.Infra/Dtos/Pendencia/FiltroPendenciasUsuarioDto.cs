namespace SME.SGP.Infra
{
    public class FiltroPendenciasUsuarioDto
    {
        public FiltroPendenciasUsuarioDto(string turmaId, int? tipoPendencia, string tituloPendencia)
        {
            TurmaId = turmaId;
            TipoPendencia = tipoPendencia;
            TituloPendencia = tituloPendencia;
        }

        public string TurmaId { get; set; }

        public int? TipoPendencia { get; set; }

        public string TituloPendencia { get; set; }
    }
}
