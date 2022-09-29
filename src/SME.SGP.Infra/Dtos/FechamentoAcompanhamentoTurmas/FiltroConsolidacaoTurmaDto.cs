namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoTurmaDto
    {
        public FiltroConsolidacaoTurmaDto(string turmaCodigo, int? bimestre, int pagina = 1)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
            Pagina = pagina;
        }

        public string TurmaCodigo { get; }
        public int? Bimestre { get; }
        public int Pagina { get; set; }
    }
}
