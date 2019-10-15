namespace SME.SGP.Dto
{
    public class AbrangenciaFiltroRetorno
    {
        public string AbreviacaoModalidade { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public int CodigoModalidade { get; set; }
        public string CodigoTurma { get; set; }
        public string CodigoUe { get; set; }
        public string DescricaoFiltro { get { return $"{AbreviacaoModalidade} - {NomeTurma} - {NomeUe} "; } }
        public string NomeDre { get; set; }
        public string NomeModalidade { get; set; }
        public string NomeTurma { get; set; }
        public string NomeUe { get; set; }
        public int Semestre { get; set; }
    }
}